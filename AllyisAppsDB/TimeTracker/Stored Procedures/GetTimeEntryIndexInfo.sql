CREATE PROCEDURE [TimeTracker].[GetTimeEntryIndexInfo]
	@OrganizationId INT,
	@UserId INT,
	@ProductId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;

	-- Settings is declared as a table here so that the StartOfWeek field can be used in other Select
	-- blocks lower in this same stored procedure, while also letting the settings table itself be returned
	DECLARE @Settings TABLE (
		StartOfWeek INT,
		LockDateUsed INT,
		LockDatePeriod VARCHAR(10),
		LockDateQuantity INT
	);
	INSERT INTO @Settings (StartOfWeek, LockDateUsed, LockDatePeriod, LockDateQuantity)
	SELECT [StartOfWeek], [LockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId

	-- Starting and Ending date parameters are adjusted if the input is null, using the StartOfWeek from above
	DECLARE @StartOfWeek INT;
	SET @StartOfWeek = (
		SELECT TOP 1
			[StartOfWeek]
		FROM @Settings
	)
	DECLARE @TodayDayOfWeek INT;
	SET @TodayDayOfWeek = ((6 + DATEPART(dw, GETDATE()) + @@DATEFIRST) % 7);

	IF(@StartingDate IS NULL)
	BEGIN
		DECLARE @DaysIntoWeek INT;
		IF (@TodayDayOfWeek < @StartOfWeek)
			SET @DaysIntoWeek = @StartOfWeek - @TodayDayOfWeek - 7;
		ELSE
			SET @DaysIntoWeek = @StartOfWeek - @TodayDayOfWeek;
		SET @StartingDate = DATEADD(dd, @DaysIntoWeek, GETDATE());
	END

	IF(@EndingDate IS NULL)
	BEGIN
		DECLARE @DaysLeftInWeek INT;
		IF (@TodayDayOfWeek < @StartOfWeek)
			SET @DaysLeftInWeek = @StartOfWeek - @TodayDayOfWeek - 1;
		ELSE
			SET @DaysLeftInWeek = @StartOfWeek - @TodayDayOfWeek + 6;
		SET @EndingDate = DATEADD(dd, @DaysLeftInWeek, GETDATE());
	END

	-- Begin select statements

	SELECT * FROM @Settings

	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];

	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[CreatedUtc],
			[Project].[StartUtc] as [StartDate],
			[Project].[EndUtc] as [EndDate],
			[Project].[Name] AS [ProjectName],
			[Project].[IsActive],
			[Project].[IsHourly] AS [IsHourly],
			[Organization].[Name] AS [OrganizationName],
			[Customer].[Name] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrganizationRoleId],
			[ProjectOrgId]
	FROM (
		(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
		FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId)
		AS [OrganizationUser]
		JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
		JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
		JOIN ( [Pjm].[Project]
			JOIN [Pjm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
		)
										ON [Project].[CustomerId] = [Customer].[CustomerId]
										AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
	)
	UNION ALL
	SELECT	[ProjectId],
			[CustomerId],
			0,
			[CreatedUtc],
			[StartUtc],
			[EndUtc],
			[Name],
			[IsActive],
			[IsHourly],
			(SELECT [Name] FROM [Auth].[Organization] WITH (NOLOCK) WHERE [OrganizationId] = 0),
			(SELECT [Name] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [CustomerId] = 0),
			NULL,
			0,
			0,
			0,
			[ProjectOrgId]
			FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = 0
	ORDER BY [Project].[Name]

	SELECT [User].[UserId],
		[User].[FirstName],
		[User].[LastName],
		[User].[Email]
	FROM [Auth].[User] WITH (NOLOCK) 
	LEFT JOIN [Billing].[SubscriptionUser]	WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
	WHERE 
		[Subscription].[SubscriptionId] = (
		SELECT [SubscriptionId] 
		FROM [Billing].[Subscription] WITH (NOLOCK) 
		LEFT JOIN [Billing].[Sku]		WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
		LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
		WHERE [Subscription].[OrganizationId] = @OrganizationId
			AND [Sku].[ProductId] = @ProductId
			AND [Subscription].[IsActive] = 1
		)
	ORDER BY [User].[LastName]

	SELECT DISTINCT [TimeEntryId] 
		,[User].[UserId] AS [UserId]
		,[User].[FirstName] AS [FirstName]
		,[User].[LastName] AS [LastName]
		,[User].[Email]
		,[OrganizationUser].[EmployeeId]
		,[TimeEntry].[ProjectId]
		,[TimeEntry].[PayClassId]
		,[PayClass].[Name] AS [PayClassName]
		,[Date]
		,[Duration]
		,[Description]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
	JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
	JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
	WHERE [User].[UserId] = @UserId
		AND [Date] >= @StartingDate
		AND [Date] <= @EndingDate
		AND [PayClass].[OrganizationId] = @OrganizationId
	ORDER BY [Date] ASC
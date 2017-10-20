CREATE PROCEDURE [TimeTracker].[GetTimeEntryIndexInfo]
	@organizationId INT,
	@userId INT,
	@productId INT,
	@startingDate DATE,
	@endingDate DATE
AS
	SET NOCOUNT ON;

	-- Settings is declared as a table here so that the StartOfWeek field can be used in other Select
	-- blocks lower in this same stored procedure, while also letting the settings table itself be returned
	DECLARE @settings TABLE (
		StartOfWeek INT,
		IsLockDateUsed INT,
		LockDatePeriod VARCHAR(10),
		LockDateQuantity INT
	);
	INSERT INTO @settings (StartOfWeek, IsLockDateUsed, LockDatePeriod, LockDateQuantity)
	SELECT [StartOfWeek], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId

	-- Starting and Ending date parameters are adjusted if the input is null, using the StartOfWeek from above
	DECLARE @startOfWeek INT;
	SET @startOfWeek = (
		SELECT TOP 1
			[StartOfWeek]
		FROM @settings
	)
	DECLARE @todayDayOfWeek INT;
	SET @todayDayOfWeek = ((6 + DATEPART(dw, GETDATE()) + @@dATEFIRST) % 7);

	IF(@startingDate IS NULL)
	BEGIN
		DECLARE @daysIntoWeek INT;
		IF (@todayDayOfWeek < @startOfWeek)
			SET @daysIntoWeek = @startOfWeek - @todayDayOfWeek - 7;
		ELSE
			SET @daysIntoWeek = @startOfWeek - @todayDayOfWeek;
		SET @startingDate = DATEADD(dd, @daysIntoWeek, GETDATE());
	END

	IF(@endingDate IS NULL)
	BEGIN
		DECLARE @daysLeftInWeek INT;
		IF (@todayDayOfWeek < @startOfWeek)
			SET @daysLeftInWeek = @startOfWeek - @todayDayOfWeek - 1;
		ELSE
			SET @daysLeftInWeek = @startOfWeek - @todayDayOfWeek + 6;
		SET @endingDate = DATEADD(dd, @daysLeftInWeek, GETDATE());
	END

	-- Begin select statements

	SELECT * FROM @settings

	
	SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;


	SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId ORDER BY [Date];


	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[StartUtc] as [StartDate],
			[Project].[EndUtc] as [EndDate],
			[Project].[ProjectName] AS [ProjectName],
			[Project].[IsActive],
			[Project].[IsHourly] AS [IsHourly],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[ProjectOrgId]
	FROM (
		(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
		FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @userId AND [OrganizationId] = @organizationId)
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
			[ProjectCreatedUtc],
			[StartUtc],
			[EndUtc],
			[ProjectName],
			[IsActive],
			[IsHourly],
			(SELECT [OrganizationName] FROM [Auth].[Organization] WITH (NOLOCK) WHERE [OrganizationId] = 0),
			(SELECT [CustomerName] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [CustomerId] = 0),
			NULL,
			0,
			0,
			[ProjectOrgId]
			FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = 0
	ORDER BY [Project].[ProjectName]

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
		WHERE [Subscription].[OrganizationId] = @organizationId
			AND [Sku].[ProductId] = @productId
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
		,[PayClass].[PayClassName] AS [PayClassName]
		,[Date]
		,[Duration]
		,[Description]
		,[TimeEntryStatusId]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
	JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
	JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @organizationId
	WHERE [User].[UserId] = @userId
		AND [Date] >= @startingDate
		AND [Date] <= @endingDate
		AND [PayClass].[OrganizationId] = @organizationId
	ORDER BY [Date] ASC
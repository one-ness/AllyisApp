CREATE PROCEDURE [TimeTracker].[GetTimeEntryIndexInfo]
	@OrganizationId INT,
	@UserId INT,
	@ProductId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
	SELECT [StartOfWeek], [LockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId;

	IF(SELECT COUNT(*) FROM [TimeTracker].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];

	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[CreatedUTC],
			[Project].[Name] AS [ProjectName],
			[Project].[IsActive],
			[Project].[Type] AS [PriceType],
			[Organization].[Name] AS [OrganizationName],
			[Customer].[Name] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrgRoleId],
			[ProjectOrgId]
	FROM (
		(SELECT [OrganizationId], [UserId], [OrgRoleId]
		FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId)
		AS [OrganizationUser]
		JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
		JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
		JOIN ( [Crm].[Project]
			JOIN [Crm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
		)
										ON [Project].[CustomerId] = [Customer].[CustomerId]
										AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
	)
	UNION ALL
	SELECT	[ProjectId],
			[CustomerId],
			0,
			[CreatedUTC],
			[Name],
			[IsActive],
			[Type],
			(SELECT [Name] FROM [Auth].[Organization] WHERE [OrganizationId] = 0),
			(SELECT [Name] FROM [Crm].[Customer] WHERE [CustomerId] = 0),
			NULL,
			0,
			0,
			0,
			[ProjectOrgId]
			FROM [Crm].[Project] WHERE [ProjectId] = 0
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
	JOIN [TimeTracker].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassID] = [TimeEntry].[PayClassId]
	JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
	WHERE [User].[UserId] = @UserId
		AND [Date] >= @StartingDate
		AND [Date] <= @EndingDate
		AND [PayClass].[OrganizationId] = @OrganizationId
	ORDER BY [Date] ASC
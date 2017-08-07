CREATE PROCEDURE [Pjm].[GetProjectEditInfo]
	@projectId INT,
	@subscriptionId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[IsHourly] AS [IsHourly],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @projectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)

	SELECT [ProjectUser].[UserId], [FirstName], [LastName]
	FROM [Pjm].[ProjectUser] WITH (NOLOCK) 
	LEFT JOIN [Pjm].[Project]	WITH (NOLOCK) ON [Project].[ProjectId] = [ProjectUser].[ProjectId]
	LEFT JOIN [Crm].[Customer]	WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [ProjectUser].[UserId]
	WHERE [Customer].[IsActive] = 1 
		AND [Project].[IsActive] = 1
		AND [ProjectUser].[IsActive] = 1
		AND [ProjectUser].[ProjectId] = @projectId
	ORDER BY [User].[LastName]

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @subscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = (
		SELECT TOP 1
			[OrganizationId]
		FROM [Pjm].[Project] WITH (NOLOCK)
		LEFT JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
		WHERE [ProjectId] = @projectId
	) AND [ProductRoleId] IS NOT NULL
	ORDER BY [User].[LastName]
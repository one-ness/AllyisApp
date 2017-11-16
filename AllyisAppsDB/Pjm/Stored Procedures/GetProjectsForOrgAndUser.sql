CREATE PROCEDURE [Pjm].[GetProjectsForOrgAndUser]
	@userId INT,
	@orgId INT
AS
	SELECT [P].[ProjectId],
			[P].[ProjectName],
			[P].[ProjectCode],
			[C].[CustomerName] AS CustomerName
	FROM [Pjm].[ProjectUser] AS [PU] WITH (NOLOCK)
	LEFT JOIN [Pjm].[Project] AS [P] WITH (NOLOCK) ON [P].[ProjectId] = [PU].[ProjectId]
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [PU].[UserId] = @userId AND [O].[OrganizationId] = @orgId AND [PU].[IsActive] = 1 AND [P].[IsActive] = 1

	SELECT [P].[ProjectId],
			[P].[ProjectName],
			[P].[ProjectCode],
			[C].[CustomerName] AS CustomerName
	FROM [Pjm].[Project] AS [P] WITH (NOLOCK)
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [O].[OrganizationId] = @orgId AND [P].[IsActive] = 1
	
	SELECT [FirstName],
		[LastName],
		[Email]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [User].[UserId] = @userId
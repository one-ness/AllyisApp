CREATE PROCEDURE [Crm].[GetProjectsForOrgAndUser]
	@UserId INT,
	@OrgId INT
AS
	SELECT [P].[ProjectId],
			[P].[Name],
			[P].[ProjectOrgId],
			[C].[Name] AS CustomerName
	FROM [Crm].[ProjectUser] AS [PU] WITH (NOLOCK)
	LEFT JOIN [Crm].[Project] AS [P] WITH (NOLOCK) ON [P].[ProjectId] = [PU].[ProjectId]
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [PU].[UserId] = @UserId AND [O].[OrganizationId] = @OrgId AND [PU].[IsActive] = 1 AND [P].[ISACTIVE] = 1

	SELECT [P].[ProjectId],
			[P].[Name],
			[P].[ProjectOrgId],
			[C].[Name] AS CustomerName
	FROM [Crm].[Project] AS [P] WITH (NOLOCK)
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [O].[OrganizationId] = @OrgId
	
	SELECT [FirstName],
		[LastName],
		[Email]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [User].[UserId] = @UserId
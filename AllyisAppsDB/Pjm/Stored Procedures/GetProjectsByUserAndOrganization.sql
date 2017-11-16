CREATE PROCEDURE [Pjm].[GetProjectsByUserAndOrganization]
	@userId INT,
	@orgId INT,
	@activity INT = 1
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Project].[IsActive],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[IsHourly] AS [PriceType],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerCode],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrganizationRoleId],
			[ProjectCode]
FROM (
	(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @userId AND [OrganizationId] = @orgId)
	AS [OrganizationUser]
	JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
	JOIN ( [Pjm].[Project]
		JOIN [Pjm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
	)
									ON [Project].[CustomerId] = [Customer].[CustomerId]
									AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
)
WHERE [Customer].[IsActive] >= @activity
	AND [Project].[IsActive] >= @activity
	AND [ProjectUser].[IsActive] >= @activity
UNION ALL
SELECT	[ProjectId],
		[CustomerId],
		0,
		[ProjectCreatedUtc],
		[ProjectName],
		[IsActive],
		[StartUtc],
		[EndUtc],
		[IsHourly],
		(SELECT [OrganizationName] FROM [Auth].[Organization]  WITH (NOLOCK) WHERE [OrganizationId] = 0),
		(SELECT [CustomerName] FROM [Crm].[Customer]  WITH (NOLOCK) WHERE [CustomerId] = 0),
		NULL,
		0,
		0,
		0,
		[ProjectCode]
		FROM [Pjm].[Project]  WITH (NOLOCK) WHERE [ProjectId] = 0
ORDER BY [Project].[ProjectName]
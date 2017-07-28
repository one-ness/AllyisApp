﻿CREATE PROCEDURE [Crm].[GetProjectsByUserAndOrganization]
	@UserId INT,
	@OrgId INT,
	@Activity INT = 1
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[CreatedUtc],
			[Project].[Name] AS [ProjectName],
			[Project].[IsActive],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[Type] AS [PriceType],
			[Organization].[Name] AS [OrganizationName],
			[Customer].[Name] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrganizationRoleId],
			[ProjectOrgId]
FROM (
	(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId)
	AS [OrganizationUser]
	JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
	JOIN ( [Crm].[Project]
		JOIN [Crm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
	)
									ON [Project].[CustomerId] = [Customer].[CustomerId]
									AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
)
WHERE [Customer].[IsActive] >= @Activity
	AND [Project].[IsActive] >= @Activity
	AND [ProjectUser].[IsActive] >= @Activity
	UNION ALL
SELECT	[ProjectId],
		[CustomerId],
		0,
		[CreatedUtc],
		[Name],
		[IsActive],
		[StartUtc],
		[EndUtc],
		[Type],
		(SELECT [Name] FROM [Auth].[Organization]  WITH (NOLOCK) WHERE [OrganizationId] = 0),
		(SELECT [Name] FROM [Crm].[Customer]  WITH (NOLOCK) WHERE [CustomerId] = 0),
		NULL,
		0,
		0,
		0,
		[ProjectOrgId]
		FROM [Crm].[Project]  WITH (NOLOCK) WHERE [ProjectId] = 0
ORDER BY [Project].[Name]
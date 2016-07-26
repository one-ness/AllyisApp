CREATE PROCEDURE [Crm].[GetProjectsByOrgId]
	@OrgId INT,
	@Activity INT = 1
AS
	SET NOCOUNT ON;
SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[CreatedUTC],
		[Project].[Name] AS [ProjectName],
		[Project].[IsActive],
		[Organization].[Name] AS [OrganizationName],
		[Customer].[Name] AS [CustomerName],
		[Customer].[IsActive] AS [IsCustomerActive]
		--[OrgRoleId]
FROM (
--(SELECT [OrganizationId], [OrgRoleId] FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @orgId) AS OrganizationUser
	[Auth].[Organization] WITH (NOLOCK) 
	JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
	JOIN [Crm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
)
	
WHERE [Customer].[IsActive] >= @Activity
	AND [Project].[IsActive] >= @Activity

CREATE PROCEDURE [Pjm].[GetProjectsByOrgId]
	@OrgId INT,
	@Activity INT = 1
AS
	SET NOCOUNT ON;
SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
		--[OrganizationRoleId]
FROM (
--(SELECT [OrganizationId], [OrganizationRoleId] FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @orgId) AS OrganizationUser
	[Auth].[Organization] WITH (NOLOCK) 
	JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
	JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
)
	
WHERE [Customer].[IsActive] >= @Activity
	AND [Project].[IsActive] >= @Activity

ORDER BY [Project].[ProjectName]
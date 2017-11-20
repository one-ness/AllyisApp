CREATE PROCEDURE [Pjm].[GetProjectsByOrgId]
    @orgId INT,
    @activity INT = 1
AS

SET NOCOUNT ON;

  SELECT [Project].[ProjectId],
         [Project].[CustomerId],
         [Project].[ProjectCreatedUtc],
         [Project].[EndUtc] AS [EndDate],
         [Project].[StartUtc] AS [StartDate],
         [Project].[ProjectName],
         [Project].[IsHourly],
         [Project].[ProjectCode],
         [Customer].[OrganizationId],
         [Customer].[CustomerName],
         [Customer].[CustomerCode],
         [Customer].[IsActive] AS [IsCustomerActive],
         [Organization].[OrganizationName]
    FROM [Auth].[Organization] WITH (NOLOCK) 
    JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
    JOIN [Pjm].[Project] WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
   WHERE [Customer].[IsActive] >= @activity
     AND [Organization].[OrganizationId] = @orgId
ORDER BY [Project].[ProjectName]
CREATE PROCEDURE [Pjm].[GetProjectsByUserAndOrganization]
    @userId INT,
    @orgId INT,
    @activity INT = 1
AS

SET NOCOUNT ON;

  SELECT [Project].[ProjectId],
         [Project].[CustomerId],
         [Customer].[OrganizationId],
         [Project].[ProjectCreatedUtc],
         [Project].[ProjectName],
         [Project].[StartUtc] AS [StartDate],
         [Project].[EndUtc] AS [EndDate],
         [Project].[IsHourly] AS [PriceType],
         [Organization].[OrganizationName],
         [Customer].[CustomerName],
         [Customer].[CustomerCode],
         [Customer].[IsActive] AS [IsCustomerActive],
         [ProjectUser].[IsActive] AS [IsUserActive],
         [OrganizationRoleId],
         [Project].[ProjectCode]
    FROM [Auth].[OrganizationUser] WITH (NOLOCK)
    JOIN [Auth].[Organization]     WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
    JOIN [Crm].[Customer]          WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
    JOIN [Pjm].[Project]           WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
    JOIN [Pjm].[ProjectUser]       WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId] AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
   WHERE [Customer].[IsActive] >= @activity
     AND [ProjectUser].[IsActive] >= @activity
     AND [OrganizationUser].[UserId] = @userId
     AND [Organization].[OrganizationId] = @orgId
ORDER BY [Project].[ProjectName]
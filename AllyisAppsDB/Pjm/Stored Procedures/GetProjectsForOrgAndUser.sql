CREATE PROCEDURE [Pjm].[GetProjectsForOrgAndUser]
    @userId INT,
    @orgId INT
AS

SELECT [P].[ProjectId],
       [P].[ProjectName],
       [P].[ProjectCode],
       [C].[CustomerName]
  FROM [Pjm].[ProjectUser]   AS [PU] WITH (NOLOCK)
  JOIN [Pjm].[Project]       AS [P] WITH (NOLOCK) ON [P].[ProjectId] = [PU].[ProjectId]
  JOIN [Crm].[Customer]      AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
  JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
 WHERE [PU].[UserId] = @userId
   AND [O].[OrganizationId] = @orgId
   AND [PU].[IsActive] = 1
   AND ([P].[StartUtc] IS NULL OR [P].[StartUtc] <= GETUTCDATE())
   AND ([P].[EndUtc] IS NULL OR [P].[EndUtc] >= GETUTCDATE())

SELECT [P].[ProjectId],
       [P].[ProjectName],
       [P].[ProjectCode],
       [C].[CustomerName]
  FROM [Pjm].[Project]       AS [P] WITH (NOLOCK)
  JOIN [Crm].[Customer]      AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
  JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
 WHERE [O].[OrganizationId] = @orgId 
   AND ([P].[StartUtc] IS NULL OR [P].[StartUtc] <= GETUTCDATE())
   AND ([P].[EndUtc] IS NULL OR [P].[EndUtc] >= GETUTCDATE())

SELECT [FirstName],
       [LastName],
       [Email]
  FROM [Auth].[User] WITH (NOLOCK)
 WHERE [UserId] = @userId
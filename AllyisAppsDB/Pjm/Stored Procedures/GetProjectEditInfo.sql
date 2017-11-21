CREATE PROCEDURE [Pjm].[GetProjectEditInfo]
    @projectId INT,
    @subscriptionId INT
AS

SET NOCOUNT ON;

SELECT [Project].[ProjectId],
       [Project].[CustomerId],
       [Project].[IsHourly] AS [IsHourly],
       [Project].[StartUtc] AS [StartDate],
       [Project].[EndUtc] AS [EndDate],
       [Project].[ProjectCode],
       [Project].[ProjectCreatedUtc],
       [Project].[ProjectName],
       [Customer].[CustomerName],
       [Customer].[CustomerCode],
       [Customer].[OrganizationId],
       [Organization].[OrganizationName]
  FROM [Pjm].[Project] WITH (NOLOCK)
  JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
  JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
 WHERE [Project].[ProjectId] = @projectId

  SELECT [User].[UserId],
         [User].[FirstName],
         [User].[LastName]
    FROM [Pjm].[ProjectUser] WITH (NOLOCK)
    JOIN [Pjm].[Project] WITH (NOLOCK) ON [Project].[ProjectId] = [ProjectUser].[ProjectId]
    JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
    JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [ProjectUser].[UserId]
   WHERE [Customer].[IsActive] = 1 
     AND [ProjectUser].[IsActive] = 1
     AND [ProjectUser].[ProjectId] = @projectId
ORDER BY [User].[LastName]

   SELECT [User].[UserId],
          [User].[FirstName],
          [User].[LastName],
          [OnRoles].[ProductRoleId]
     FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
LEFT JOIN (SELECT [UserId], [ProductRoleId] 
             FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
            WHERE [SubscriptionId] = @subscriptionId)
               AS [OnRoles]
               ON [OnRoles].[UserId] = [User].[UserId]
    WHERE [OrganizationId] = (
              SELECT TOP 1 [OrganizationId]
                FROM [Pjm].[Project] WITH (NOLOCK)
                JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
               WHERE [ProjectId] = @projectId)
      AND [OnRoles].[ProductRoleId] IS NOT NULL
 ORDER BY [User].[LastName]
CREATE PROCEDURE [Crm].[GetProjectByIdAndUser]
    @projectId int,
    @userId int
AS

SET NOCOUNT ON;

   SELECT [p].[ProjectId],
          [p].[CustomerId],
          [p].[ProjectCreatedUtc],
          [p].[ProjectName],
          [p].[IsHourly] AS [PriceType],
          [p].[StartUtc] AS [StartDate],
          [p].[EndUtc] AS [EndDate],
          [p].[ProjectCode],
          [c].[OrganizationId],
          [c].[CustomerName],
          [c].[CustomerCode],
          [Organization].[OrganizationName],
          [SUB].[IsProjectUser]
     FROM [Pjm].[Project] [p] WITH (NOLOCK)
     JOIN [Crm].[Customer] [c] WITH (NOLOCK) ON [c].[CustomerId] = [p].[CustomerId]
     JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [c].[OrganizationId]
LEFT JOIN (SELECT 1 AS [IsProjectUser],
                  [pu].[ProjectId]
             FROM [Pjm].[ProjectUser] [pu] WITH (NOLOCK)
            WHERE [pu].[ProjectId] = @projectId AND [pu].[UserId] = @userId) [SUB]
               ON [SUB].[ProjectId] = @projectId
    WHERE [p].[ProjectId] = @projectId
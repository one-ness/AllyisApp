CREATE PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
	@UserId [Auth].[UserTable] READONLY,
	@OrganizationId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId] 
	  ,[User].[UserId] AS [UserId]
	  ,[User].[FirstName] AS [FirstName]
	  ,[User].[LastName] AS [LastName]
      ,[TimeEntry].[ProjectId]
	  ,[PayClassId]
      ,[Date]
      ,[Duration]
      ,[Description]
  FROM [TimeTracker].[TimeEntry]
  WITH (NOLOCK) 
  LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
    LEFT JOIN [Crm].[Project] WITH (NOLOCK) ON [Project].[ProjectId] = [TimeEntry].[ProjectId]
	LEFT JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].CustomerId = [Project].[CustomerId]
  WHERE [User].[UserId] IN (SELECT [userId] FROM @UserId)
  AND [Date] >= @StartingDate
  AND [Date] <= @EndingDate
  AND [Customer].[OrganizationId] = @OrganizationId
  ORDER BY [Date] ASC
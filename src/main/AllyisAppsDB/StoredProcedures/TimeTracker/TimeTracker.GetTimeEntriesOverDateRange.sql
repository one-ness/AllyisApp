CREATE PROCEDURE [TimeTracker].[GetTimeEntriesOverDateRange]
	@OrganizationId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId] 
	  ,[User].[UserId] AS [UserId]
	  ,[User].[FirstName] AS [FirstName]
	  ,[User].[LastName] AS [LastName]
      ,[Project].[ProjectId]
	  ,[PayClassId]
      ,[Date]
      ,[Duration]
      ,[Description]
  FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
  JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
  JOIN [Crm].[Customer] WITH (NOLOCK) ON ([Customer].[OrganizationId] = @OrganizationId OR [Customer].[OrganizationId] = 0)
  JOIN [Crm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[UserId] = [TimeEntry].[UserId]
  JOIN [Crm].[Project] WITH (NOLOCK) ON ([Project].[ProjectId] = [TimeEntry].[ProjectId] OR [Project].[ProjectId] = 0)
  WHERE [Date] >= @StartingDate
  AND [Date] <= @EndingDate
  AND [Project].[CustomerId] IN (SELECT [Customer].[CustomerId] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [Customer].[OrganizationId] = @OrganizationId) OR [Customer].[OrganizationId] = 0
  ORDER BY [Date] ASC
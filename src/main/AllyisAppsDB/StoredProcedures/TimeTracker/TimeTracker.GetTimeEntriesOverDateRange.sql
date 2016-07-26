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
  JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[OrganizationId] = @OrganizationId
  JOIN [Crm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[UserId] = [TimeEntry].[UserId]
  JOIN [Crm].[Project] WITH (NOLOCK) ON [Project].[ProjectId] = [TimeEntry].[ProjectId]
  WHERE [Date] >= @StartingDate
  AND [Date] <= @EndingDate
  AND [Project].[CustomerId] IN (SELECT [Customer].[CustomerId] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [Customer].[OrganizationId] = @OrganizationId)
  ORDER BY [Date] ASC
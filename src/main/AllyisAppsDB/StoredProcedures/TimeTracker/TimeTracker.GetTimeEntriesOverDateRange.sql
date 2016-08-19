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
    ,[TimeEntry].[ProjectId]
	,[TimeEntry].[PayClassId]
    ,[Date]
    ,[Duration]
    ,[Description]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
JOIN [TimeTracker].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassID] = [TimeEntry].[PayClassId]
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
WHERE [Date] >= @StartingDate
	AND [Date] <= @EndingDate
	AND [PayClass].[OrganizationId] = @OrganizationId
ORDER BY [Date] ASC
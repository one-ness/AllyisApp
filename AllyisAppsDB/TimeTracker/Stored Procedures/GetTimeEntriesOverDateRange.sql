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
	,[User].[Email]
	,[OrganizationUser].[EmployeeId]
    ,[TimeEntry].[ProjectId]
	,[TimeEntry].[PayClassId]
	,[PayClass].[Name] AS [PayClassName]
    ,[Date]
    ,[Duration]
    ,[Description]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
WHERE [Date] >= @StartingDate
	AND [Date] <= @EndingDate
	AND [PayClass].[OrganizationId] = @OrganizationId
ORDER BY [Date] ASC
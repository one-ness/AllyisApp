CREATE PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
	@userId [Auth].[UserTable] READONLY,
	@organizationId INT,
	@startingDate DATE,
	@endingDate DATE
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
	,[PayClass].[PayClassName] AS [PayClassName]
	,[Date]
	,[Duration]
	,[Description]
	,[TimeEntryStatusId]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @organizationId
WHERE [User].[UserId] IN (SELECT [userId] FROM @userId)
	AND [Date] >= @startingDate
	AND [Date] <= @endingDate
	AND [PayClass].[OrganizationId] = @organizationId
ORDER BY [Date] ASC
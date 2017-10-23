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
	,[IsLocked] = CAST(
		CASE
			WHEN CAST([TimeEntry].[Date] AS DATE) <= CAST([Setting].[LockDate] AS DATE) -- TODO: turn [TimeEntry].[Date] to DATE type
				THEN 1
			ELSE 0
			END
		AS BIT)
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @organizationId
JOIN [TimeTracker].[Setting] [Setting] WITH (NOLOCK) ON [Setting].[OrganizationId] = [OrganizationUser].[OrganizationId]
WHERE [User].[UserId] IN (SELECT [userId] FROM @userId)
	AND [Date] >= @startingDate
	AND [Date] <= @endingDate
	AND [PayClass].[OrganizationId] = @organizationId
ORDER BY [Date] ASC
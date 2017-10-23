CREATE PROCEDURE [TimeTracker].[GetTimeEntriesOverDateRange]
	@organizationId INT,
	@startingDate DATE,
	@endingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT 
	[te].[TimeEntryId]
	,[te].[ProjectId]
	,[te].[PayClassId]
	,[te].[Date]
	,[te].[Duration]
	,[te].[Description]
	,[te].[TimeEntryStatusId]
	,[u].[UserId] AS [UserId]
	,[u].[FirstName] AS [FirstName]
	,[u].[LastName] AS [LastName]
	,[u].[Email]
	,[ou].[EmployeeId]
	,[pc].[PayClassName] AS [PayClassName]
	,[IsLocked] = CAST(
		CASE
			WHEN CAST([te].[Date] AS DATE) <= CAST([s].[LockDate] AS DATE)  -- TODO: turn [TimeEntry].[Date] to DATE type
				THEN 1
			ELSE 0
			END
		AS BIT)
FROM [TimeTracker].[TimeEntry] [te] WITH (NOLOCK)
JOIN [Hrm].[PayClass] [pc] WITH (NOLOCK) ON [pc].[PayClassId] = [te].[PayClassId]
JOIN [Auth].[User] [u] WITH (NOLOCK) ON [u].[UserId] = [te].[UserId]
JOIN [Auth].[OrganizationUser] [ou] WITH (NOLOCK) ON [u].[UserId] = [ou].[UserId] AND [ou].[OrganizationId] = @organizationId
JOIN [TimeTracker].[Setting] [s] WITH (NOLOCK) ON [s].[OrganizationId] = [ou].[OrganizationId]
WHERE [te].[Date] >= @startingDate
	AND [te].[Date] <= @endingDate
	AND [pc].[OrganizationId] = @organizationId
ORDER BY [Date] ASC
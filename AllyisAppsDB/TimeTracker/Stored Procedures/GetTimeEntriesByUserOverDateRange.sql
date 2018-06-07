CREATE PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
    @userId [Auth].[UserTable] READONLY,
    @organizationId INT,
    @startingDate DATE,
    @endingDate DATE
AS
SET NOCOUNT ON;
SELECT DISTINCT
          [te].[TimeEntryId]
         ,[te].[Date]
         ,[te].[Duration]
         ,[te].[Description]
         ,[te].[TimeEntryStatusId]
         ,[te].[ProjectId]
         ,[te].[PayClassId]
         ,[u].[UserId] AS [UserId]
         ,[u].[FirstName] AS [FirstName]
         ,[u].[LastName] AS [LastName]
         ,[u].[Email]
         ,[ou].[EmployeeId]
         ,[pc].[PayClassName] AS [PayClassName]
		 ,[pc].[BuiltinPayClassId]
         ,[IsLocked] = CAST(
             CASE
                 WHEN ([s].[LockDate] IS NOT NULL
                         AND [te].[Date] <= [s].[LockDate])
                     OR ([s].[LockDate] IS NULL
                         AND [te].[Date] <= [s].[PayrollProcessedDate])
                 THEN 1
                 ELSE 0
             END
         AS BIT)
    FROM [TimeTracker].[TimeEntry] [te] WITH (NOLOCK) 
    JOIN [Auth].[User]              [u] WITH (NOLOCK) ON [u].[UserId] = [te].[UserId]
    JOIN [Hrm].[PayClass]          [pc] WITH (NOLOCK) ON [pc].[PayClassId] = [te].[PayClassId]
    JOIN [Auth].[OrganizationUser] [ou] WITH (NOLOCK) ON [u].[UserId] = [ou].[UserId] AND [ou].[OrganizationId] = @organizationId
    JOIN [TimeTracker].[Setting]    [s] WITH (NOLOCK) ON [s].[OrganizationId] = [ou].[OrganizationId]
   WHERE [u].[UserId] IN (SELECT [userId] FROM @userId)
     AND [te].[Date] >= @startingDate
     AND [te].[Date] <= @endingDate
     AND [pc].[OrganizationId] = @organizationId
ORDER BY [te].[Date] ASC
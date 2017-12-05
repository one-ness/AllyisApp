CREATE PROCEDURE [TimeTracker].[UpdateTimeEntry]
	@timeEntryId INT,
	@projectId INT,
	@payClassId INT,
	@duration FLOAT,
	@description NVARCHAR(120),
	@timeEntryStatusId INT
AS

SET NOCOUNT ON;

-- Get mergeable time entries
DECLARE @userId INT
DECLARE @date DATE
DECLARE @mergeableTimeEntryId INT
DECLARE @mergeableDuration FLOAT
DECLARE @oldDuration FLOAT
DECLARE @sumOfMergeable FLOAT

SELECT  @userId = [UserId],
		@date = [Date]
FROM [TimeTracker].[TimeEntry]
WHERE [TimeEntryId] = @timeEntryId

SELECT  [TimeEntryId]
INTO #MergeableTimeEntries
FROM [TimeTracker].[TimeEntry]
WHERE [UserId] = @userId
AND [ProjectId] = @projectId
AND [Date] = @date
AND [PayClassId] = @payClassId
AND [TimeEntryId] != @timeEntryId

SELECT @sumOfMergeable = ISNULL(SUM([Duration]), 0)
FROM [TimeTracker].[TimeEntry]
WHERE [TimeEntryId] IN ( SELECT [TimeEntryId] FROM #MergeableTimeEntries)

-- Execute merge
;WITH [UpdatedEntry] AS (
	SELECT 
		[TimeEntryId] = @timeEntryId,
		[Duration] = @sumOfMergeable + @duration,
		[Description] = @description,
		[ProjectId] = @projectId,
		[PayClassId] = @payClassId,
		[Delete] = 0
	UNION ALL
	SELECT 
		[TimeEntryId],
		null,
		null,
		null,
		null,
		[Delete] = 1
	FROM #MergeableTimeEntries
)
MERGE [TimeTracker].[TimeEntry] WITH (HOLDLOCK) AS [T]
USING [UpdatedEntry] AS [S]
ON [T].[TimeEntryId] = [S].[TimeEntryId]
WHEN MATCHED AND [Delete] = 0 THEN
	UPDATE SET
		[T].[Duration] = [S].[Duration],
		[T].[TimeEntryStatusId] = 0,
		[T].[Description] = [S].[Description],
		[T].[ProjectId] = [S].[ProjectId],
		[T].[PayClassId] = [S].[PayClassId]
WHEN MATCHED THEN DELETE;

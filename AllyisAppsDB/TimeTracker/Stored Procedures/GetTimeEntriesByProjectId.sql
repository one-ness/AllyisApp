CREATE PROCEDURE [TimeTracker].[GetTimeEntriesByProjectId]
	@projectId INT
AS
	SET NOCOUNT ON;
	SELECT
		[UserId],
		[ProjectId],
		[PayClassId],
		[Date],
		[Duration],
		[Description],
		[IsLockSaved],
		[TimeEntryStatusId]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	WHERE [ProjectId] = @projectId
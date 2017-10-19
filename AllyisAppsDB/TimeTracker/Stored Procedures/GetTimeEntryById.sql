CREATE PROCEDURE [TimeTracker].[GetTimeEntryById]
	@timeEntryId INT
AS
	SET NOCOUNT ON;
	SELECT [UserId],
		[ProjectId],
		[PayClassId],
		[Date],
		[Duration],
		[Description],
		[IsLockSaved],
		[TimeEntryStatusId]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	WHERE [TimeEntryId] = @timeEntryId
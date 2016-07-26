CREATE PROCEDURE [TimeTracker].[GetTimeEntryById]
	@TimeEntryId INT
AS
	SET NOCOUNT ON;
	SELECT [UserId],
		[ProjectId],
		[PayClassId],
		[Date],
		[Duration],
		[Description],
		[LockSaved]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	WHERE [TimeEntryId] = @TimeEntryId
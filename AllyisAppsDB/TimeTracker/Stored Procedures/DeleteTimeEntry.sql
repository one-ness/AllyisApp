CREATE PROCEDURE [TimeTracker].[DeleteTimeEntry]
	@timeEntryId INT
AS
	SET NOCOUNT ON;
DELETE [TimeTracker].[TimeEntry] 
    WHERE [TimeEntryId] = @timeEntryId
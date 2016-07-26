CREATE PROCEDURE [TimeTracker].[DeleteTimeEntry]
	@TimeEntryId INT
AS
	SET NOCOUNT ON;
DELETE [TimeTracker].[TimeEntry] 
    WHERE [TimeEntryId] = @TimeEntryId
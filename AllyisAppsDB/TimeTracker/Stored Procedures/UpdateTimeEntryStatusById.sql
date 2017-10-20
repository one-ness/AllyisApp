CREATE PROCEDURE [TimeTracker].[UpdateTimeEntryStatusById]
	@timeEntryId INT,
	@timeEntryStatusId INT
AS
UPDATE [TimeTracker].[TimeEntry] SET [TimeEntryStatusId] = @timeEntryStatusId WHERE [TimeEntryId] = @timeEntryId
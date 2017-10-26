CREATE PROCEDURE [TimeTracker].[UpdateTimeEntryStatusById]
    @timeEntryId INT,
    @timeEntryStatusId INT
AS
    UPDATE [t]
       SET [t].[TimeEntryStatusId] = @timeEntryStatusId
      FROM [TimeTracker].[TimeEntry] [t]
     WHERE [t].[TimeEntryId] = @timeEntryId

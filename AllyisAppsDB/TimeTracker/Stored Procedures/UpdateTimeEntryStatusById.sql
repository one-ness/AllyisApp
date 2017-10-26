CREATE PROCEDURE [TimeTracker].[UpdateTimeEntryStatusById]
    @timeEntryId INT,
    @timeEntryStatusId INT
AS
    UPDATE [t]
       SET [t].[TimeEntryStatusId] = @timeEntryStatusId
      FROM [TimeTracker].[TimeEntry] [t]
      JOIN [Pjm].[Project] [p] ON [p].[ProjectId] = [t].[ProjectId]
      JOIN [Crm].[Customer] [c] on [c].[CustomerId] = [p].[CustomerId]
      JOIN [TimeTracker].[Setting] [s] ON [s].[OrganizationId] = [c].[OrganizationId]
     WHERE [t].[TimeEntryId] = @timeEntryId
       AND [t].[Date] > [s].[LockDate]

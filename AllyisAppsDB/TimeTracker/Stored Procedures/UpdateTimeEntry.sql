CREATE PROCEDURE [TimeTracker].[UpdateTimeEntry]
	@timeEntryId INT,
    @projectId INT,
	@payClassId INT,
	@duration FLOAT,
	@description NVARCHAR(120),
	@isLockSaved BIT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[TimeEntry]
   SET [ProjectId] = @projectId
	  ,[PayClassId] = @payClassId
      ,[Duration] = @duration
      ,[Description] = @description
	  ,[IsLockSaved] = @isLockSaved
 WHERE [TimeEntryId] = @timeEntryId
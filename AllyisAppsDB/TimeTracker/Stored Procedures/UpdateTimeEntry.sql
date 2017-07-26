CREATE PROCEDURE [TimeTracker].[UpdateTimeEntry]
	@TimeEntryId INT,
    @ProjectId INT,
	@PayClassId INT,
	@Duration FLOAT,
	@Description NVARCHAR(120),
	@LockSaved BIT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[TimeEntry]
   SET [ProjectId] = @ProjectId
	  ,[PayClassId] = @PayClassId
      ,[Duration] = @Duration
      ,[Description] = @Description
	  ,[LockSaved] = @LockSaved
 WHERE [TimeEntryId] = @TimeEntryId
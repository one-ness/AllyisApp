CREATE PROCEDURE [TimeTracker].[UpdateTimeEntry]
	@TimeEntryId INT,
    @ProjectId INT,
	@PayClassID INT,
	@Duration FLOAT,
	@Description NVARCHAR(120),
	@LockSaved BIT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[TimeEntry]
   SET [ProjectId] = @ProjectId
	  ,[PayClassId] = @PayClassID
      ,[Duration] = @Duration
      ,[Description] = @Description
	  ,[LockSaved] = @LockSaved
 WHERE [TimeEntryId] = @TimeEntryId
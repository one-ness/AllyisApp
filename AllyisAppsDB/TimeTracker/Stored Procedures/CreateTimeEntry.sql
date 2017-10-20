CREATE PROCEDURE [TimeTracker].[CreateTimeEntry]
	@userId INT,
	@projectId INT,
	@payClassId INT,
	@date DATE,
	@duration FLOAT,
	@description NVARCHAR(120),
	@timeEntryStatusId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON
	INSERT INTO [TimeTracker].[TimeEntry]
			   ([UserId]
			   ,[ProjectId]
			   ,[PayClassId]
			   ,[Date]
			   ,[Duration]
			   ,[Description]
			   ,[TimeEntryStatusId])
		 VALUES(@userId
			   ,@projectId
			   ,@payClassId
			   ,@date
			   ,@duration
			   ,@description
			   ,@timeEntryStatusId);
	SELECT SCOPE_IDENTITY();
END
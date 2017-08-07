CREATE PROCEDURE [TimeTracker].[CreateTimeEntry]
	@userId INT,
	@projectId INT,
	@payClassId INT,
    @date DATE,
    @duration FLOAT,
    @description NVARCHAR(120)
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
			   ,[Description])
		 VALUES(@userId
			   ,@projectId
			   ,@payClassId
			   ,@date
			   ,@duration
			   ,@description);
	SELECT SCOPE_IDENTITY();
END
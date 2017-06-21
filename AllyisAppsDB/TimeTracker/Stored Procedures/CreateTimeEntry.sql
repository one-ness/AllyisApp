CREATE PROCEDURE [TimeTracker].[CreateTimeEntry]
	@UserId INT,
	@ProjectId INT,
	@PayClassId INT,
    @Date DATE,
    @Duration FLOAT,
    @Description NVARCHAR(120)
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
		 VALUES(@UserId
			   ,@ProjectId
			   ,@PayClassId
			   ,@Date
			   ,@Duration
			   ,@Description);
	SELECT SCOPE_IDENTITY();
END
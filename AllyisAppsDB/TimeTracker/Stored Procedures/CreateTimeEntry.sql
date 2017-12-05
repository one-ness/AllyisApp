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

	DECLARE @id INT;

	WITH [NewEntry] AS (
		SELECT 
			[UserId] = @userId,
			[ProjectId] = @projectId,
			[PayClassId] = @payClassId,
			[Date] = @date,
			[Duration] = @duration,
			[Description] = @description,
			[TimeEntryStatusId] = @timeEntryStatusId
	)
	MERGE [TimeTracker].[TimeEntry] WITH (HOLDLOCK) AS [T]
	USING [NewEntry] AS [S]
	ON [T].[UserId] = [S].[UserId]
	AND [T].[ProjectId] = [S].[ProjectId]
	AND [T].[Date] = [S].[Date]
	AND [T].[PayClassId] = [S].[PayClassId]
	WHEN MATCHED THEN
		UPDATE SET
			[T].[Duration] = [T].[Duration] + [S].[Duration],
			[T].[TimeEntryStatusId] = 0,
			@id = [T].[TimeEntryId]
	WHEN NOT MATCHED THEN
		INSERT 
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

	SELECT ISNULL(@id, SCOPE_IDENTITY());
END
CREATE PROCEDURE [Staffing].[CreatePositionTags]
	@tags [Lookup].[TagTable] READONLY,
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRANSACTION
		-- COMMENT: Insert the tags into [Tag] if they don't already exist there
		INSERT INTO [Lookup].[Tag] ([TagName])
		SELECT [NEWTAGS].[TagName]
		FROM @tags AS [NEWTAGS]
		WHERE NOT EXISTS
			(SELECT [TagName]
			FROM [Lookup].[Tag]
			WHERE [Tag].[TagName] = [NEWTAGS].[TagName])

		-- Get the tag ids of the newly created tags from the previous statement
		DECLARE @tagIds TABLE ([TagId] INT)
		INSERT INTO @tagIds
		SELECT [TagId]
		FROM [Lookup].[Tag]
		WHERE [Tag].[TagName] IN
			(SELECT [TagName] FROM @tags)
	
		-- Insert all the new tags into [PositionTag]
		INSERT INTO [Staffing].[PositionTag] ([PositionId], [TagId])
		SELECT @positionId, [TagId]
		FROM @tagIds
	COMMIT TRANSACTION
END

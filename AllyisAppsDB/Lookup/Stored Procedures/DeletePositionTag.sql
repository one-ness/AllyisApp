CREATE PROCEDURE [Lookup].[DeletePositionTag]
	@tagId INT,
	@positionId INT
	
AS
BEGIN
	DELETE FROM [Lookup].[PositionTag] WHERE [TagId] = @tagId AND [PositionId] = @positionId
END
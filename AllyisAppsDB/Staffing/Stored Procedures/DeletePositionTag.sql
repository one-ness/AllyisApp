CREATE PROCEDURE [Staffing].[DeletePositionTag]
	@tagId INT,
	@positionId INT
	
AS
BEGIN
	DELETE FROM [Staffing].[PositionTag] WHERE [TagId] = @tagId AND [PositionId] = @positionId
END
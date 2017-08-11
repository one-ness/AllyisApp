CREATE PROCEDURE [StaffingManager].[DeletePositionTag]
	@tagId INT,
	@positionId INT
	
AS
BEGIN
	DELETE FROM [StaffingManager].[PositionTag] WHERE [TagId] = @tagId AND [PositionId] = @positionId
END
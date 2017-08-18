CREATE PROCEDURE [StaffingManager].[DeletePositionLevel]
	@positionLevelId INT
	
AS
BEGIN
	DELETE FROM [StaffingManager].[PositionLevel] WHERE [PositionLevelId] = @positionLevelId
END
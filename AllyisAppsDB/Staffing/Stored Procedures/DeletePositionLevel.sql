CREATE PROCEDURE [Staffing].[DeletePositionLevel]
	@positionLevelId INT
	
AS
BEGIN
	DELETE FROM [Staffing].[PositionLevel] WHERE [PositionLevelId] = @positionLevelId
END
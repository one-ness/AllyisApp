CREATE PROCEDURE [StaffingManager].[DeletePosition]
	@positionId INT
	
AS
	DELETE FROM [StaffingManager].[Position]
		WHERE [PositionId] IN (SELECT [PositionId] FROM @positionId) 
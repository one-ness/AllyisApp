CREATE PROCEDURE [StaffingManager].[DeletePosition]
	@PositionId INT
	
AS
	DELETE FROM [StaffingManager].[Position]
		WHERE [PositionId] IN (SELECT [PositionId] FROM @PositionId) 
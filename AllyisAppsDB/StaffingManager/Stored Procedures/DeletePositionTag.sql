CREATE PROCEDURE [StaffingManager].[DeletePositionTag]
	@tagId INT,
	@positionId INT
	
AS
	DELETE FROM [StaffingManager].[PositionTag]
		WHERE [TagId] IN (SELECT [TagId] FROM @TagId) AND [PositionId] = @positionId
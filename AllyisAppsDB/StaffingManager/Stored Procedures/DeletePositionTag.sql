CREATE PROCEDURE [StaffingManager].[DeletePositionTag]
	@TagId INT,
	@PositionId INT
	
AS
	DELETE FROM [StaffingManager].[PositionTag]
		WHERE [TagId] IN (SELECT [TagId] FROM @TagId) AND [PositionId] = @PositionId
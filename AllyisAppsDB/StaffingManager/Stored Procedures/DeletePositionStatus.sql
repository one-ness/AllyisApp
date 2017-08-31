CREATE PROCEDURE [StaffingManager].[DeletePositionStatus]
	@positionStatusId INT
	
AS
BEGIN
	DELETE FROM [StaffingManager].[PositionStatus] WHERE [PositionStatusId] = @positionStatusId
END
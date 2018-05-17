CREATE PROCEDURE [Staffing].[DeletePositionStatus]
	@positionStatusId INT
	
AS
BEGIN
	DELETE FROM [Staffing].[PositionStatus] WHERE [PositionStatusId] = @positionStatusId
END
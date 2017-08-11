CREATE PROCEDURE [StaffingManager].[DeletePosition]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [StaffingManager].[Position] WHERE [PositionId] = @positionId
END

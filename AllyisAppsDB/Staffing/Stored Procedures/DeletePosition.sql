CREATE PROCEDURE [Staffing].[DeletePosition]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [Staffing].[Position] WHERE [PositionId] = @positionId
END

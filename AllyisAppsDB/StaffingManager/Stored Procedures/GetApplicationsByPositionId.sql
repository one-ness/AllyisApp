CREATE PROCEDURE [StaffingManager].[GetApplicationsByPositionId]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Application] WHERE [PositionId] = @positionId
END

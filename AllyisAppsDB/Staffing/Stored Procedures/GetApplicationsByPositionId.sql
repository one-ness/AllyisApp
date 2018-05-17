CREATE PROCEDURE [Staffing].[GetApplicationsByPositionId]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[Application] WHERE [PositionId] = @positionId
END

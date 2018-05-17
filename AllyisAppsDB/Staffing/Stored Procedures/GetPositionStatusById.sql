CREATE PROCEDURE [Staffing].[GetPositionStatusById]
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [Staffing].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @positionStatusId
END
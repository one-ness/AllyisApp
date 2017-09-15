CREATE PROCEDURE [StaffingManager].[GetPositionStatusById]
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [StaffingManager].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @positionStatusId
END
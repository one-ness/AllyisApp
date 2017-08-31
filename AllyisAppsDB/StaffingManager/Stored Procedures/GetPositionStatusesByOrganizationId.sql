CREATE PROCEDURE [StaffingManager].[GetPositionStatusesByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [StaffingManager].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[PositionStatus].[PositionStatusName] DESC
END
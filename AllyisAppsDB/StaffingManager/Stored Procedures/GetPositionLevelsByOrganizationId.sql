CREATE PROCEDURE [StaffingManager].[GetPositionLevelsByorganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [StaffingManager].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[PositionLevel].[PositionLevelName] DESC
END
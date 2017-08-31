CREATE PROCEDURE [StaffingManager].[GetPositionLevelById]
	@PositionLevelId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [StaffingManager].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @PositionLevelId
END
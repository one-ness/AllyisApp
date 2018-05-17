CREATE PROCEDURE [Staffing].[GetPositionLevelsByorganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [Staffing].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId
	ORDER BY [Staffing].[PositionLevel].[PositionLevelName] DESC
END
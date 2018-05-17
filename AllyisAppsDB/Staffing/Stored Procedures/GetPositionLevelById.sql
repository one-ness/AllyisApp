CREATE PROCEDURE [Staffing].[GetPositionLevelById]
	@PositionLevelId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [Staffing].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @PositionLevelId
END
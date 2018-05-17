CREATE PROCEDURE [Staffing].[GetPositionStatusesByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [Staffing].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
	ORDER BY [Staffing].[PositionStatus].[PositionStatusName] DESC
END
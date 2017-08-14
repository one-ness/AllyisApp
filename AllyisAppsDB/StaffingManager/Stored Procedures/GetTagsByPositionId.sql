CREATE PROCEDURE [StaffingManager].[GetTagsByPositionId]
	@positionId INT
AS
BEGIN
	SELECT [StaffingManager].[Tag].[TagName]
	FROM [StaffingManager].[Tag]
	JOIN [StaffingManager].[PositionTag] WITH (NOLOCK) ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [StaffingManager].[PositionTag].[PositionId] = @positionId
END
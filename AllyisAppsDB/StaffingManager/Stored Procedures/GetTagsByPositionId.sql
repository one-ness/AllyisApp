CREATE PROCEDURE [StaffingManager].[GetTagsByPositionId]
	@positionId INT
AS
BEGIN
	SELECT
		@positionId AS [PositionId],
		[T].[TagId],
		[T].[TagName]
	FROM [StaffingManager].[Tag] [T]
		JOIN [StaffingManager].[PositionTag] [PT] ON [PT].[TagId] = [T].[TagId]
	WHERE [PT].[PositionId] = @positionId
END
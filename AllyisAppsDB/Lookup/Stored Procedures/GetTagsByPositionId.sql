CREATE PROCEDURE [Lookup].[GetTagsByPositionId]
	@positionId INT
AS
BEGIN
	SELECT
		[T].[TagId],
		[T].[TagName]
	FROM [Lookup].[Tag] [T]
		JOIN [Lookup].[PositionTag] [PT] ON [PT].[TagId] = [T].[TagId]
	WHERE [PT].[PositionId] = @positionId
END
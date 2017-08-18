CREATE PROCEDURE [Lookup].[GetPositionTagsByPosition]
	@PositionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		@PositionId AS [PositionId],
		[PositionTag].[TagId],
		[Tag].[TagName]
	FROM [Lookup].[PositionTag]
		JOIN [Lookup].[Tag]
		ON [Tag].[TagId] = [PositionTag].[TagId]
	WHERE [PositionTag].[PositionId] = @PositionId; 
END
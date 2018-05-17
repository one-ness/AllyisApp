CREATE PROCEDURE [Staffing].[GetPositionTagsByPosition]
	@PositionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		@PositionId AS [PositionId],
		[PositionTag].[TagId],
		[Tag].[TagName]
	FROM [Staffing].[PositionTag]
		JOIN [Lookup].[Tag]
		ON [Tag].[TagId] = [PositionTag].[TagId]
	WHERE [PositionTag].[PositionId] = @PositionId; 
END
CREATE PROCEDURE [StaffingManager].[GetPositionTagsByPosition]
	@PositionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		@PositionId AS [PositionId],
		[PositionTag].[TagId],
		[Tag].[TagName]
	FROM [StaffingManager].[PositionTag]
		JOIN [StaffingManager].[Tag]
		ON [Tag].[TagId] = [PositionTag].[TagId]
	WHERE [PositionTag].[PositionId] = @PositionId; 
END
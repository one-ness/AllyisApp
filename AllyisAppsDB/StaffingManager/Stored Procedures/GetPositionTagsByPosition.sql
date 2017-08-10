CREATE PROCEDURE [StaffingManager].[GetPositionTagsByPosition]
	@PositionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [PositionTag].[TagId],
		   [Tag].[TagName]
	FROM [StaffingManager].[PositionTag] WITH (NOLOCK)
	JOIN [StaffingManager].[Tag]		 WITH (NOLOCK) ON [Tag].[TagId] = [Positiontag].[TagId]
	WHERE [Tag].[TagId] = [PositionTag].[TagId]; 

	DECLARE @TagId INT
	SET @TagId = (SELECT U.TagId
					 FROM [StaffingManager].[Tag] AS U
					 WHERE [TagId] = @TagId)
END
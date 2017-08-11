CREATE PROCEDURE [StaffingManager].[CreatePositionTag]
	@tagId INT,
	@positionId INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[PositionTag] 
		([TagId], 
		[PositionId])
	VALUES 	
		(@tagId, 
		@positionId)

END

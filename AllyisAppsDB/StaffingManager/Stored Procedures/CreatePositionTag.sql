CREATE PROCEDURE [StaffingManager].[CreatePositionTag]
	@TagId INT,
	@PositionId INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[PositionTag] 
		([TagId], 
		[PositionId])
	VALUES 	
		(@TagId, 
		@PositionId)

END

CREATE PROCEDURE [Staffing].[CreatePositionTag]
	@tagId INT,
	@positionId INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [Staffing].[PositionTag] 
		([TagId], 
		[PositionId])
	VALUES 	
		(@tagId, 
		@positionId)
END

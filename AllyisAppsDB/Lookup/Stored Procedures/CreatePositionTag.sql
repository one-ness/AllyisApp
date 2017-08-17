CREATE PROCEDURE [Lookup].[CreatePositionTag]
	@tagId INT,
	@positionId INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [Lookup].[PositionTag] 
		([TagId], 
		[PositionId])
	VALUES 	
		(@tagId, 
		@positionId)

END

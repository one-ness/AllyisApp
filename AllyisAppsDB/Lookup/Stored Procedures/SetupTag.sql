CREATE PROCEDURE [Lookup].[SetupTag]
	@positionId INT,
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		EXEC [Lookup].[CreateTag] @tagName;

		DECLARE @tagId INT 
		SET @tagId = IDENT_CURRENT('[Lookup].[TagId]');

		EXEC [StaffingManager].[CreatePositionTag] @tagId, @positionId;

		SELECT IDENT_CURRENT('[Lookup].[TagId]');
	COMMIT TRANSACTION
END

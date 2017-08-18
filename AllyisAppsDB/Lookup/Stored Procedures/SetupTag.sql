CREATE PROCEDURE [Lookup].[SetupTag]
	@positionId INT,
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		EXEC [Lookup].[CreateTag] @tagName;

		DECLARE @tagId INT = IDENT_CURRENT('[StaffingManager].[TagId]');

		EXEC [Lookup].[CreatePositionTag] @tagId, @positionId;

		SELECT IDENT_CURRENT('[StaffingManager].[TagId]');
	COMMIT TRANSACTION
END

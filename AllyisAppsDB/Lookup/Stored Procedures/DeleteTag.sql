CREATE PROCEDURE [Lookup].[DeleteTag]
	@tagId INT
	
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [Lookup].[Tag] WHERE [TagId] = @tagId
END

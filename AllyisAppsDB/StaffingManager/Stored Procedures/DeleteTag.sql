CREATE PROCEDURE [StaffingManager].[DeleteTag]
	@tagId INT
	
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [StaffingManager].[Tag] WHERE [TagId] = @tagId
END

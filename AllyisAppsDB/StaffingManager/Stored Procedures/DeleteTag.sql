CREATE PROCEDURE [StaffingManager].[DeleteTag]
	@tagId INT
	
AS
	DELETE FROM [StaffingManager].[Tag]
		WHERE [TagId] IN (SELECT [TagId] FROM @tagId) 
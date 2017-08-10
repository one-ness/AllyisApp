CREATE PROCEDURE [StaffingManager].[DeleteTag]
	@TagId INT
	
AS
	DELETE FROM [StaffingManager].[Tag]
		WHERE [TagId] IN (SELECT [TagId] FROM @TagId) 
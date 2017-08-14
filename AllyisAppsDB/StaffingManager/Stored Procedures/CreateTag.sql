CREATE PROCEDURE [StaffingManager].[CreateTag]
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[Tag] 
		([TagName])
	VALUES 	 
		(@tagName)
END

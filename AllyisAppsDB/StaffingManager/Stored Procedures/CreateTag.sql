CREATE PROCEDURE [StaffingManager].[CreateTag]
	@tagName INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[Tag] 
		([TagName])
	VALUES 	 
		(@tagName)
END

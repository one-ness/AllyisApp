CREATE PROCEDURE [StaffingManager].[CreateTag]
	@TagName INT

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[Tag] 
		([TagName])
	VALUES 	 
		(@TagName)

END

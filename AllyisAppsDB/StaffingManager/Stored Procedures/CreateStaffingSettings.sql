CREATE PROCEDURE [StaffingManager].[CreateStaffingSettings]
	@organizationId		INT

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [StaffingManager].[StaffingSettings] 
		([OrganizationId],
		[DefaultPositionStatusId])
	VALUES 	 
		(@organizationId,
		NULL)
END

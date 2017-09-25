CREATE PROCEDURE [StaffingManager].[UpdateStaffingSettings]
	@organizationId INT,
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [StaffingManager].[StaffingSettings] 
	SET 
		[DefaultPositionStatusId] = @positionStatusId
	WHERE [StaffingSettings].[OrganizationId] = @organizationId
END

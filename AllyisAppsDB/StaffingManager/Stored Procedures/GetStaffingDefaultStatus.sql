CREATE PROCEDURE [StaffingManager].[GetStaffingDefaultStatus]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [DefaultPositionStatusId],
		   [DefaultApplicationStatusId]
	FROM [StaffingManager].[StaffingSettings]
	WHERE [StaffingSettings].[OrganizationId] = @organizationId
END
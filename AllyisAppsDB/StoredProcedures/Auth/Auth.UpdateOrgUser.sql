CREATE PROCEDURE [Auth].[UpdateOrgUser]
	@OrganizationId INT,
	@UserId INT,
	@RoleId INT,
	@EmployeeId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[OrganizationUser] SET [OrgRoleId] = @RoleId, [EmployeeId] = @EmployeeId
	WHERE [OrganizationId] = @OrganizationId AND [UserId] = @UserId
END

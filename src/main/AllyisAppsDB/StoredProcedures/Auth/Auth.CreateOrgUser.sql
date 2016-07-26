CREATE PROCEDURE [Auth].[CreateOrgUser]
    @UserId INT,
    @OrganizationId INT,
    @RoleId INT,
	@EmployeeId NVARCHAR(128) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[OrganizationUser]
	SET [OrgRoleId] = @RoleId, 
		[EmployeeId] = @EmployeeId
	WHERE [UserId] = @UserId AND 
		[OrganizationId] = @OrganizationId;

	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO [Auth].[OrganizationUser] 
			([UserId], 
			[OrganizationId], 
			[OrgRoleId], 
			[EmployeeId])
		VALUES 
			(@UserId, 
			@OrganizationId,
			@RoleId, 
			@EmployeeId);
	END
END

CREATE PROCEDURE [Auth].[CreateOrgUser]
    @UserId INT,
    @OrganizationId INT,
    @RoleId INT,
	@EmployeeId NVARCHAR(128) = NULL,
	@EmployeeTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[OrganizationUser]
	SET [OrgRoleId] = @RoleId, 
		[EmployeeId] = @EmployeeId,
		[EmployeeTypeId] = @EmployeeTypeId
	WHERE [UserId] = @UserId AND 
		[OrganizationId] = @OrganizationId;

	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO [Auth].[OrganizationUser] 
			([UserId], 
			[OrganizationId], 
			[OrgRoleId], 
			[EmployeeId],
			[EmployeeTypeId])
		VALUES 
			(@UserId, 
			@OrganizationId,
			@RoleId, 
			@EmployeeId,
			@EmployeeTypeId);
	END
END

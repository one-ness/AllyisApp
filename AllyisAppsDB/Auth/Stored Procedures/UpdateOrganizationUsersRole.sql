CREATE PROCEDURE [Auth].[UpdateOrganizationUsersRole]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@organizationRole INT
AS
BEGIN
	UPDATE [Auth].[OrganizationUser]
	SET [OrganizationRoleId] = @organizationRole
	WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [OrganizationId] = @organizationId
END

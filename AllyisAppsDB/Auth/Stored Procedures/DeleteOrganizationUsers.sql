CREATE PROCEDURE [Auth].[DeleteOrganizationUsers]
	@organizationId	INT,
	@userIds [Auth].[UserTable] READONLY
AS
	DELETE FROM [Auth].[OrganizationUser]
	WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [OrganizationId] = @organizationId

CREATE PROCEDURE [Auth].[UpdateUserActiveOrg]
	@userId INT,
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [LastUsedOrganizationId] = @organizationId
	WHERE [UserId] = @userId
END

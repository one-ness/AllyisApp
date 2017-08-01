CREATE PROCEDURE [Auth].[UpdateUserActiveOrg]
	@UserId INT,
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [LastUsedOrganizationId] = @OrganizationId
	WHERE [UserId] = @UserId
END
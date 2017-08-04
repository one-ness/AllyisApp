CREATE PROCEDURE [Auth].[GetOrganizationOwnerEmails]
	@OrganizationId INT
AS
BEGIN
	SELECT [User].[Email]
	FROM [Auth].[OrganizationUser]
	JOIN [Auth].[User]
	ON [OrganizationUser].[UserId] = [User].[UserId]
	WHERE [OrganizationRoleId] = 2 AND @OrganizationId = [OrganizationId]
End
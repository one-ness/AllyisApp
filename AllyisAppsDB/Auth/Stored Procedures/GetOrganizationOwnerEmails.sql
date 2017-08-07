CREATE PROCEDURE [Auth].[GetOrganizationOwnerEmails]
	@organizationId INT
AS
BEGIN
	SELECT [User].[Email]
	FROM [Auth].[OrganizationUser]
	JOIN [Auth].[User]
	ON [OrganizationUser].[UserId] = [User].[UserId]
	WHERE [OrganizationRoleId] = 2 AND @organizationId = [OrganizationId]
End
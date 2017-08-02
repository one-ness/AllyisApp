CREATE PROCEDURE [Auth].[GetOrganizationOwnerEmails]
	@OrganizationId INT
AS
	SELECT [User].[Email]
	FROM [Auth].[OrganizationUser]
	JOIN [Auth].[User]
	ON [OrganizationUser].[UserId] = [User].[UserId]
	WHERE [OrganizationRoleId] = 2
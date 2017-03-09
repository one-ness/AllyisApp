CREATE PROCEDURE [Auth].[GetOrgUserRole]
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[OrgRole].[OrgRoleId], 
		[OrgRole].[Name]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrgRole] WITH (NOLOCK) ON [OrgRole].[OrgRoleId] = [OrganizationUser].[OrgRoleId]
	JOIN [Auth].[User]			WITH (NOLOCK) ON [User].[UserId] = @UserId
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
		AND [OrganizationUser].[UserId] = @UserId
END

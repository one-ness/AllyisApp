CREATE PROCEDURE [Auth].[GetOrgUserRole]
	@organizationId INT,
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[OrganizationRole].[OrganizationRoleId], 
		[OrganizationRole].[OrganizationRoleName]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	JOIN [Auth].[User]			WITH (NOLOCK) ON [User].[UserId] = @userId
	WHERE [OrganizationUser].[OrganizationId] = @organizationId
		AND [OrganizationUser].[UserId] = @userId
END
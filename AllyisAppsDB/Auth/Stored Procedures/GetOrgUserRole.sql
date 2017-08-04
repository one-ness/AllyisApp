CREATE PROCEDURE [Auth].[GetOrgUserRole]
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[OrganizationRole].[OrganizationRoleId], 
		[OrganizationRole].[OrganizationRoleName]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	JOIN [Auth].[User]			WITH (NOLOCK) ON [User].[UserId] = @UserId
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
		AND [OrganizationUser].[UserId] = @UserId
END
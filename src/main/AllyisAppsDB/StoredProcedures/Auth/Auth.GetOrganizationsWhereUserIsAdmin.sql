CREATE PROCEDURE [Auth].[GetOrganizationsWhereUserIsAdmin]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Name], [Organization].[OrganizationId]
	FROM [Auth].[OrganizationUser]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [OrganizationUser].[OrganizationId]
	JOIN [Auth].[User]				WITH (NOLOCK) ON [User].[UserId] = @UserId
	WHERE [OrganizationUser].[UserId] = @UserId
		AND [OrganizationUser].[OrgRoleId] = 2
		AND [Organization].[IsActive] = 1
	ORDER BY [Organization].[Name]
END
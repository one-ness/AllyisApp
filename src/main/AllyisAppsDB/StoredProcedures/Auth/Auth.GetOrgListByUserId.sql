CREATE PROCEDURE [Auth].[GetOrgListByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[O].[OrganizationId],
		[UserId],
		[Name] AS [OrganizationName],
		[OrgRoleId]
	FROM [Auth].[OrganizationUser]		AS [OU]
	WITH (NOLOCK)
	INNER JOIN [Auth].[Organization]	AS [O] WITH (NOLOCK) 
		ON [O].[OrganizationId] = [OU].[OrganizationId]
	WHERE [OU].[UserId] = @UserId
		AND [O].[IsActive] = 1
	ORDER BY [O].[Name]
END

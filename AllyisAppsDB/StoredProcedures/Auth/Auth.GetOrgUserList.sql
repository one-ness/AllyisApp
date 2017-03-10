CREATE PROCEDURE [Auth].[GetOrgUserList]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OU].[OrganizationId],
	       [OU].[UserId],
		   [OU].[OrgRoleId],
		   [O].[Name] AS [OrganizationName],
		   [OU].[EmployeeId],
		   [U].[Email]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @OrganizationId
	ORDER BY [U].[LastName]
END

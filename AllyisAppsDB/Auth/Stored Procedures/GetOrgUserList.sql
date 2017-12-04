CREATE PROCEDURE [Auth].[GetOrgUserList]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OU].[OrganizationId],
	       [OU].[UserId],
		   [OU].[OrganizationRoleId],
		   [O].[OrganizationName] AS [OrganizationName],
		   [OU].[EmployeeId],
		   [U].[Email],
		   [U].[FirstName],
		   [U].[LastName],
		   [OU].EmployeeTypeId,
		   [OU].OrganizationUserCreatedUtc as 'CreatedUtc'
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @organizationId
	ORDER BY [U].[LastName]
END
CREATE PROCEDURE [Auth].[GetOrgUserList]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OU].[OrganizationId],
	       [OU].[UserId],
		   [OU].[OrgRoleId],
		   [U].[FirstName],
		   [U].[LastName],
		   [OU].[EmployeeId]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
    WHERE [OU].[OrganizationId] = @OrganizationId
END

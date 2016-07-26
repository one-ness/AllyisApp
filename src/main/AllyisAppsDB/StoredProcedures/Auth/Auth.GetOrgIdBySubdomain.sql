CREATE PROCEDURE [Auth].[GetOrgIdBySubdomain]
	@Subdomain NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 [OrganizationId] 
	FROM [Auth].[Organization]
	WITH (NOLOCK) 
	WHERE [Subdomain] = @Subdomain AND [IsActive] = 1
END
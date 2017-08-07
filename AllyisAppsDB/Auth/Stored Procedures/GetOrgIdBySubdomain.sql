CREATE PROCEDURE [Auth].[GetOrgIdBySubdomain]
	@subdomain NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 [OrganizationId] 
	FROM [Auth].[Organization]
	WITH (NOLOCK) 
	WHERE [Subdomain] = @subdomain AND [IsActive] = 1
END
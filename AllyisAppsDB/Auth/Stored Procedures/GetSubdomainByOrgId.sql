CREATE PROCEDURE [Auth].[GetSubdomainByOrgId]
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 [Subdomain] 
	FROM [Auth].[Organization]
	WITH (NOLOCK)
	WHERE [OrganizationId] = @orgId;
END
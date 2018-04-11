CREATE PROCEDURE [Auth].[GetOrganization]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	select * from Organization with (nolock)
	where OrganizationId = @organizationId
END
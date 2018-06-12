CREATE procedure [Auth].[GetOrganizationUsers]
	@organizationId int
as
begin
	set nocount on
	select * from OrganizationUser with (nolock)
	where OrganizationId = @organizationId
end
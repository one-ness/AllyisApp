create procedure Auth.[GetOrgUserCount]
	@organizationId int
as
begin
	set nocount on
	select count(UserId) as 'UserCount' from OrganizationUser with (nolock)
	where OrganizationId = @organizationId
end
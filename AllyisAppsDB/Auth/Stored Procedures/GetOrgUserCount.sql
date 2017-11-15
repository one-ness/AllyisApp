create procedure Auth.[GetOrgUserCount]
	@orgId int
as
begin
	set nocount on
	select count(UserId) as 'UserCount' from OrganizationUser with (nolock)
	where OrganizationId = @orgId
end
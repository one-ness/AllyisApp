
CREATE procedure [Auth].[GetUserOrganizations]
	@userId int
as
begin
	set nocount on
	select o.* from Organization o with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	where ou.UserId = @userId and o.IsActive = 1
end
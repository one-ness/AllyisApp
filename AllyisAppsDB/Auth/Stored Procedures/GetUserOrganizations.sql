
CREATE procedure [Auth].[GetUserOrganizations]
	@userId int
as
begin
	set nocount on
	select o.*, a.* from Organization o with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	inner join [Lookup].[Address] a with (nolock) on a.AddressId = o.AddressId
	where ou.UserId = @userId and o.IsActive = 1
end
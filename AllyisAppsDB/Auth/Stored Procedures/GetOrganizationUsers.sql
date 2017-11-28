create procedure [Auth].[GetOrganizationUsers]
	@organizationId int
as
begin
	set nocount on
		-- get user information with address
	select u.*, a.*, ou.*, s.StateName as 'State', c.CountryName as 'Country' from [User] u with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.UserId = u.UserId
	inner join Organization o with (nolock) on o.OrganizationId = ou.OrganizationId
	left join [Lookup].[Address] a with (nolock) on a.AddressId = u.AddressId
	left join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	left join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where ou.OrganizationId = @organizationId and o.IsActive = 1
end
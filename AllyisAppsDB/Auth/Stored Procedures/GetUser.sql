create procedure Auth.GetUser
	@userId int
as
begin
	set nocount on

	-- get user information with address
	select u.*, a.*, s.StateName, c.CountryName from [User] u with (nolock)
	left join [Lookup].[Address] a with (nolock) on a.AddressId = u.AddressId
	left join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	left join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where u.UserId = @userId

	-- get list of organizations and the user role in each
	select o.*, ou.*, a.* from Organization o with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	left join Lookup.Address a with (nolock) on a.AddressId = o.AddressId
	where ou.UserId = @userId

	-- get a list of subscriptions and the user role in each
	select s.*, su.*, sku.SkuId, sku.SkuName, p.ProductId, p.ProductName, p.AreaUrl from Billing.Subscription s with (nolock)
	inner join Billing.SubscriptionUser su with (nolock) on su.SubscriptionId = s.SubscriptionId
	inner join Organization o with (nolock) on o.OrganizationId = s.OrganizationId
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	inner join Billing.Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Billing.Product p with (nolock) on p.ProductId = sku.ProductId
	where ou.UserId = @userId and su.UserId = @userId and o.IsActive = 1 and s.IsActive = 1
end
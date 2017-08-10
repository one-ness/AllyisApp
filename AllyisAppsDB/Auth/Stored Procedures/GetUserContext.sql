CREATE procedure [Auth].[GetUserContext]
	@userId int
as
begin
	set nocount on
	-- return 3 result sets
	-- get user information
	select u.FirstName, u.LastName, u.UserId, u.Email, u.PreferredLanguageId from [User] u with (nolock)
	left join OrganizationUser ou with (nolock) on ou.UserId = u.UserId

	-- get list of organizations and the user role in each
	create table #OrgAndRole(OrganizationId int, OrganizationRoleId int, OrganizationName nvarchar(64))
	insert into #OrgAndRole(OrganizationId, OrganizationRoleId, OrganizationName) select ou.OrganizationId, ou.OrganizationRoleId, o.OrganizationName from OrganizationUser ou with (nolock)
	inner join Organization o with (nolock) on o.OrganizationId = ou.OrganizationId
	where ou.UserId = @userId
	select * from #OrgAndRole with (nolock)

	-- get list of subscriptions of those organizations
	select su.SubscriptionId, s.SubscriptionName, su.ProductRoleId, pr.ProductRoleName, sku.SkuId, sku.SkuName, p.ProductId, p.ProductName, p.AreaUrl, s.OrganizationId from Billing.SubscriptionUser su with (nolock)
	inner join Billing.Subscription s with (nolock) on s.SubscriptionId = su.SubscriptionId
	inner join #OrgAndRole orgrole with (nolock) on orgrole.OrganizationId = s.OrganizationId
	inner join Billing.Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Billing.Product p with (nolock) on p.ProductId = sku.ProductId
	inner join ProductRole pr with (nolock) on pr.ProductId = p.ProductId

	-- drop the temp table
	drop table #OrgAndRole
end
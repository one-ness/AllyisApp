CREATE procedure [Auth].[GetUserContext]
	@userId int
as
begin
	set nocount on
	-- return 3 result sets
	-- get user information
	select u.FirstName, u.LastName, u.UserId, u.Email, u.PreferredLanguageId, u.LoginProviderId from [User] u with (nolock)
	where u.UserId = @userId;

	-- get list of organizations and the user role in each
	create table #OrgAndRole(OrganizationId int, OrganizationRoleId int, OrganizationName nvarchar(64), MaxAmount decimal)
	insert into #OrgAndRole(OrganizationId, OrganizationRoleId, OrganizationName, MaxAmount) select ou.OrganizationId, ou.OrganizationRoleId, o.OrganizationName, ou.MaxAmount from OrganizationUser ou with (nolock)
	inner join Organization o with (nolock) on o.OrganizationId = ou.OrganizationId
	where ou.UserId = @userId and o.IsActive = 1
	select * from #OrgAndRole with (nolock)

	-- get the subscriptions of those organizations and the role of the user in those subscriptions
	select s.SubscriptionId, s.SubscriptionName,
	sku.SkuId, sku.SkuName, p.ProductId, p.ProductName, p.AreaUrl, su.ProductRoleId, s.OrganizationId from Billing.Subscription s with (nolock)
	inner join #OrgAndRole orgrole with (nolock) on orgrole.OrganizationId = s.OrganizationId
	inner join Billing.Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Billing.Product p with (nolock) on p.ProductId = sku.ProductId
	inner join Billing.SubscriptionUser su with (nolock) on su.SubscriptionId = s.SubscriptionId
	where s.IsActive = 1 and su.UserId = @userId

	-- drop the temp table
	drop table #OrgAndRole
end
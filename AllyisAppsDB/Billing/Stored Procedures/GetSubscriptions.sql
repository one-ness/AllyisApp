create procedure Billing.GetSubscriptions
	@orgId int
as
begin
	set nocount on
	select s.*, sk.SkuName, p.ProductId, p.ProductName, p.AreaUrl, p.Description as 'ProductDescription' from subscription s with (nolock)
	inner join sku sk with (nolock) on sk.SkuId = s.SkuId
	inner join product p with (nolock) on p.ProductId = sk.ProductId
	where s.OrganizationId = @orgId and s.IsActive = 1 and sk.IsActive = 1 and p.IsActive = 1
end
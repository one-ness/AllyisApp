CREATE PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
begin
	SET NOCOUNT ON;
	select s.*, sku.SkuName, sku.[Description] as 'SkuDescription', sku.ProductId, sku.IconUrl, p.AreaUrl, p.[Description] as 'ProductDescription', p.ProductName from Subscription s with (nolock)
	inner join Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Product p with (nolock) on p.ProductId = sku.ProductId
	where s.SubscriptionId = @subscriptionId and s.IsActive = 1 and sku.IsActive = 1 and p.IsActive = 1
end
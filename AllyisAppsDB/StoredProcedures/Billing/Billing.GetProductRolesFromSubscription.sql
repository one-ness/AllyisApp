CREATE PROCEDURE [Billing].[GetProductRolesFromSubscription]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
SELECT 
	[ProductRole].[Name],
	[ProductRole].[ProductRoleId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
WHERE [SubscriptionId] = @SubscriptionId

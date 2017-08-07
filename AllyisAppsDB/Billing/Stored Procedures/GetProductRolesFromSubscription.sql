CREATE PROCEDURE [Billing].[GetProductRolesFromSubscription]
	@subscriptionId INT
AS
	SET NOCOUNT ON;
SELECT 
	[ProductRole].[ProductRoleName],
	[ProductRole].[ProductRoleId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
WHERE [SubscriptionId] = @subscriptionId
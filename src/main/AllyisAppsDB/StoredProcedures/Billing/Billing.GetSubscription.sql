CREATE PROCEDURE [Billing].[GetSubscription]
	@OrganizationId INT,
	@ProductId INT
AS
	SET NOCOUNT ON;
SELECT
	[SubscriptionId],
	[Sku].[SkuId],
	[NumberOfUsers]
FROM [Billing].[Subscription] WITH (NOLOCK) 
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
WHERE [OrganizationId] = @OrganizationId AND [ProductId] = @ProductId AND [Subscription].[IsActive] = 1

CREATE PROCEDURE [Billing].[GetProductAreaBySubscription]
	@subscriptionId INT
AS
	SET NOCOUNT ON;
	SELECT DISTINCT [Product].[ProductName]
	FROM [Billing].[Product] WITH (NOLOCK) 
	INNER JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[ProductId] = [Product].[ProductId]
	INNER JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SkuId] = [Sku].[SkuId]
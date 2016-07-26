CREATE PROCEDURE [Billing].[GetSubscriptionByOrgAndProduct]
	@OrganizationId INT,
	@ProductId INT
AS
	SET NOCOUNT ON;
SELECT 
	[Name],
	[CostPerBlock],
	[UserLimit],
	[BillingFrequency],
	[Subscription].[SubscriptionId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Sku].[ProductId] = @ProductId
	AND [Subscription].[IsActive] = 1
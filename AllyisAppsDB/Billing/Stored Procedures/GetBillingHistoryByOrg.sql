CREATE PROCEDURE [Billing].[GetBillingHistoryByOrg]
	@organizationId INT
AS
	SET NOCOUNT ON;
SELECT
	[BillingHistory].[OrganizationId],
	[BillingHistory].[BillingHistoryCreatedUtc] AS [Date],
	[BillingHistory].[Description],
	[BillingHistory].[UserId],
	[BillingHistory].[SkuId],
	[Sku].[SkuName] AS [SkuName],
	[Sku].[ProductId],
	[Product].[ProductName] AS [ProductName]
FROM [Billing].[BillingHistory] WITH (NOLOCK)
LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [BillingHistory].[UserId]
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [BillingHistory].[SkuId]
LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
WHERE [OrganizationId] = @organizationId
ORDER BY [Date] desc
CREATE PROCEDURE [Billing].[GetBillingHistoryByOrg]
	@OrganizationID INT
AS
	SET NOCOUNT ON;
SELECT
	[BillingHistory].[OrganizationId],
	[BillingHistory].[CreatedUTC] AS [Date],
	[BillingHistory].[Description],
	[BillingHistory].[UserId],
	[BillingHistory].[SkuId],
	[Sku].[Name] AS [SkuName],
	[Sku].[ProductId],
	[Product].[Name] AS [ProductName]
FROM [Billing].[BillingHistory] WITH (NOLOCK)
LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [BillingHistory].[UserId]
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [BillingHistory].[SkuId]
LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
WHERE [OrganizationId] = @OrganizationID
ORDER BY [Date] desc
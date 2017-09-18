CREATE PROCEDURE [Billing].[GetSubscriptionsDisplayByOrg]
	@organizationId INT
AS
	SET NOCOUNT ON;
SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
  FROM [Billing].[Subscription] WITH (NOLOCK) 
  LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
  LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
  LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
  WHERE [Subscription].[OrganizationId] = @organizationId
	AND [Subscription].[IsActive] = 1
ORDER BY [Product].[ProductName]
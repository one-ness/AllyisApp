CREATE PROCEDURE [Billing].[GetSubscriptionsDisplayByOrg]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
SELECT	[Product].[ProductId],
		[Product].[Name] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[Name] AS [OrganizationName],
		[Sku].[Name] AS [SkuName],
		(SELECT COUNT([UserId])
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
			WHERE [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId])
			AS SubscriptionsUsed,
		[Tier]
  FROM [Billing].[Subscription] WITH (NOLOCK) 
  LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
  LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
  LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
  WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1

CREATE PROCEDURE [Billing].[GetSubscriptionByUser]
	@UserId INT
AS
	SET NOCOUNT ON;
SELECT
	[Organization].[Name] AS [OrganizationName],
	[Sku].[Name],
	[Organization].[CreatedUTC],
	[Subscription].[SubscriptionId],
	[Organization].[OrganizationId],
	[Subscription].[SkuId]
FROM (SELECT [SubscriptionId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
		WHERE [UserId] = @UserId) AS [Src]
JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [Src].[SubscriptionId]
JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
WHERE [Subscription].[IsActive] = 1
CREATE PROCEDURE [Billing].[GetOrgSkus]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[A1].[SubscriptionId],
		[A1].[ProductName],
		[A2].[UserCount],
		[A1].[SkuId]
	FROM
	(
		SELECT
			[OrgSub].[OrganizationId],
			[OrgSub].[SkuId],
			[ProductName],
			[SubscriptionId]
		FROM [Billing].[Subscription] AS [OrgSub] WITH (NOLOCK) 
		INNER JOIN 
		(
			SELECT
				[Sku].[SkuId],
				[Product].[ProductName]
			FROM [Billing].[Product] AS [Product] WITH (NOLOCK) 
			INNER JOIN [Billing].[Sku] AS [Sku] WITH (NOLOCK) 
			ON [Product].[ProductId] = [Sku].[ProductId]
		) AS [ProductSku]
		ON [ProductSku].[SkuId] = [OrgSub].[SkuId] AND [OrgSub].[IsActive] = 1
		WHERE [OrgSub].[OrganizationId] = @OrganizationId
	) AS [A1]
	INNER JOIN
	(
		SELECT [SubUser].[SubscriptionId], COUNT(*) AS [UserCount]
		FROM [Billing].[SubscriptionUser] AS [SubUser] WITH (NOLOCK) 
		INNER JOIN [Billing].[Subscription] AS [OrgSubs] WITH (NOLOCK) 
		ON [OrgSubs].[SubscriptionId] = [SubUser].[SubscriptionId]
		WHERE [OrgSubs].[OrganizationId] = @OrganizationId AND [OrgSubs].[IsActive] = 1
		GROUP BY [SubUser].[SubscriptionId]
	) AS [A2]
	ON [A1].[SubscriptionId] = [A2].[SubscriptionId]
	ORDER BY [A1].[ProductName]
END
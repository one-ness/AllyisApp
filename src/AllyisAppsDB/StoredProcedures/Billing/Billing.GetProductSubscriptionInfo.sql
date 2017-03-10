CREATE PROCEDURE [Billing].[GetProductSubscriptionInfo]
	@productId INT,
	@orgId INT
AS
	SET NOCOUNT ON;
	SELECT [Product].[Name], [Product].[ProductId], [Product].[Description]
	  FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductId] = @productId

	SELECT
		[SubscriptionId],
		[Sku].[SkuId],
		[NumberOfUsers]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	WHERE [OrganizationId] = @orgId AND [ProductId] = @productId AND [Subscription].[IsActive] = 1

	SELECT [SkuId]
		,[ProductId]
		,[Name]
		,[CostPerBlock]
		,[UserLimit]
		,[BillingFrequency]
		,[Tier]
		,[EntityName]
		,[BlockSize]
		,[PromoCostPerBlock]
		,[PromoDeadline]
		,[IsActive]
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [Billing].[Sku].[ProductId] = @productId

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1

	SELECT COUNT([UserId])
	FROM (
		SELECT [SubscriptionUser].[UserId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
		LEFT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		WHERE 
			[Subscription].[SubscriptionId] = (
			SELECT [SubscriptionId] 
			FROM [Billing].[Subscription] WITH (NOLOCK) 
			LEFT JOIN [Billing].[Sku]		WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
			LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
			WHERE [Subscription].[OrganizationId] = @orgId
				AND [Sku].[ProductId] = @productId
				AND [Subscription].[IsActive] = 1
			)
	) src
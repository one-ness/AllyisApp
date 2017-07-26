CREATE PROCEDURE [Billing].[GetProductSubscriptionInfo]
	@skuId INT,
	@orgId INT
AS
	SET NOCOUNT ON;
	DECLARE @ProductId INT;
	DECLARE @SubscriptionId INT;

SELECT @ProductId = [Product].[ProductId]
FROM [Billing].[Product] 
	  LEFT JOIN [Billing].[Sku] WITH (NOLOCK) 
	  ON [Product].ProductId = [Sku].ProductId	  
	  WHERE [Sku].SkuId = @skuId

SELECT 
	[Product].[Name], 
	[Product].[ProductId], 
	[Product].[Description], 
	[Product].[AreaUrl]
	FROM [Billing].[Product]   
	WHERE [Product].ProductId = @ProductId

	SELECT
		@SubscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [Subscription].[SkuId] = @skuId AND [Subscription].[IsActive] = 1

	SELECT
		[SubscriptionId],
		[SkuId],
		[NumberOfUsers],
		[CreatedUtc],
		[OrganizationId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [SubscriptionId] = @SubscriptionId

	SELECT [SkuId],
		[ProductId],
		[Name],
		[CostPerBlock],
		[UserLimit],
		[BillingFrequency],
		[BlockBasedOn],
		[BlockSize],
		[PromoCostPerBlock],
		[PromoDeadline],
		[IsActive],
		[Description]
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [Billing].[Sku].[ProductId] = @ProductId

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1

	SELECT COUNT([UserId])
	FROM (
		SELECT [SubscriptionUser].[UserId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
		LEFT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		WHERE 
			[Subscription].[SubscriptionId] = @SubscriptionId
	) src
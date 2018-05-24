CREATE PROCEDURE [Billing].[GetProductSubscriptionInfo]
	@productId int,
	@orgId INT
AS
	SET NOCOUNT ON;
	DECLARE @subscriptionId INT;

SELECT 
	[Product].[ProductName], 
	[Product].[ProductId], 
	[Product].[Description], 
	[Product].[AreaUrl]
	FROM [Billing].[Product]   
	WHERE [Product].ProductId = @productId

	SELECT
		@subscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [Subscription].[ProductId] = @productId --AND [Subscription].[IsActive] = 1

	SELECT
		[SubscriptionId],
		[UserCount],
		[SubscriptionCreatedUtc],
		[OrganizationId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [SubscriptionId] = @subscriptionId

	SELECT [SkuId],
		[ProductId],
		[SkuName],
		[CostPerBlock],
		[UserLimit],
		[BillingFrequency],
		[BlockBasedOn],
		[BlockSize],
		[PromoCostPerBlock],
		[PromoDeadline],
		[IsActive],
		[Description],
		[IconUrl]
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [Billing].[Sku].[ProductId] = @productId

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId --AND [IsActive] = 1

	SELECT COUNT([UserId])
	FROM (
		SELECT [SubscriptionUser].[UserId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
		LEFT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		WHERE 
			[Subscription].[SubscriptionId] = @subscriptionId
	) src
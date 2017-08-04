CREATE PROCEDURE [Billing].[DeleteSubscriptionUsers]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@productId INT
AS
BEGIN TRANSACTION
	-- get the subscription id from the productId and orgId provided
	DECLARE @subscriptionId INT;
	SELECT
		@subscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId] -- @productId
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Product].[ProductId] = @productId AND [Subscription].[IsActive] = 1


	DELETE [Billing].[SubscriptionUser] 
	WHERE [SubscriptionId] = @subscriptionId AND [UserId] IN (SELECT [userId] FROM @userIds)
COMMIT TRANSACTION

CREATE PROCEDURE [Billing].[DeleteSubscription]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Delete subscription
	UPDATE [Billing].[Subscription]
	SET [IsActive] = 0
	WHERE [SubscriptionId] = @subscriptionId

	-- Return sku name for the notification
	SELECT [Sku].[SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Subscription].[SkuId] = [Sku].[SkuId]
	WHERE [Subscription].[SubscriptionId] = @subscriptionId
END
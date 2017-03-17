CREATE PROCEDURE [Billing].[DeleteSubscription]
	@SubscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Delete subscription
	UPDATE [Billing].[Subscription]
	SET [IsActive] = 0
	WHERE [SubscriptionId] = @SubscriptionId

	-- Return sku name for the notification
	SELECT [Sku].[Name]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Subscription].[SkuId] = [Sku].[SkuId]
	WHERE [Subscription].[SubscriptionId] = @SubscriptionId
END
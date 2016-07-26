CREATE PROCEDURE [Billing].[DeleteSubscription]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
	UPDATE [Billing].[Subscription]
	SET [IsActive] = 0
	WHERE [SubscriptionId] = @SubscriptionId
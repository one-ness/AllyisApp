CREATE PROCEDURE [Billing].[DeactivateSubscription]
	@subscriptionId INT
AS
	SET NOCOUNT ON;

	UPDATE [Billing].[Subscription]
	SET [IsActive] = 0
	WHERE [SubscriptionId] = @subscriptionId
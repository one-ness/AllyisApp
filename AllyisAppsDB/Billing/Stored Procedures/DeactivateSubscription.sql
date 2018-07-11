CREATE PROCEDURE [Billing].[DeactivateSubscription]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	Begin Transaction
	DELETE FROM SubscriptionUser WHERE [SubscriptionId] = @subscriptionId
	DELETE FROM Subscription WHERE [SubscriptionId] = @subscriptionId
	--UPDATE [Billing].[Subscription]
	--SET [SubscriptionStatus] = 0
	--WHERE [SubscriptionId] = @subscriptionId
	Commit Transaction
END
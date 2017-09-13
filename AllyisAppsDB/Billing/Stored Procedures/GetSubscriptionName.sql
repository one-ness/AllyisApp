CREATE PROCEDURE [Billing].[GetSubscriptionName]
	@subScriptionId int
AS
	SELECT Subscription.SubscriptionName
	FROM 
	Billing.Subscription
	Where Subscription.SubscriptionId = @subScriptionId

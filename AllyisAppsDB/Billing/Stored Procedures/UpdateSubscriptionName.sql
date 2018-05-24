-- TODO: pass in subscriptionId as a parameter to simplify logic

CREATE PROCEDURE [Billing].[UpdateSubscriptionName]
	@subscriptionId INT,
	@subscriptionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Billing].[Subscription] SET [SubscriptionName] = @subscriptionName
	where [Subscription].SubscriptionId = @subscriptionId --and Subscription.IsActive = 1
END
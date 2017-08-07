CREATE PROCEDURE [Billing].[DeleteSubscriptionUser]
	@subscriptionId INT,
	@userId INT
AS
	SET NOCOUNT ON;
	DELETE [Billing].[SubscriptionUser] 
	WHERE [SubscriptionId] = @subscriptionId AND [UserId] = @userId;
CREATE PROCEDURE [Billing].[DeleteSubscriptionUser]
	@SubscriptionId INT,
	@UserId INT
AS
	SET NOCOUNT ON;
	DELETE [Billing].[SubscriptionUser] 
	WHERE [SubscriptionId] = @SubscriptionId AND [UserId] = @UserId;

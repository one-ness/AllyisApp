CREATE PROCEDURE [Billing].[UpdateSubscriptionUserProductRole]
	@productRoleId INT,
	@subscriptionId INT,
	@userId INT
AS
	UPDATE [Billing].[SubscriptionUser] 
		SET [ProductRoleId] = @productRoleId
			WHERE [SubscriptionId] = @subscriptionId AND [UserId] = @userId;
	IF @@ROWCOUNT=0
		BEGIN
			INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId]) 
			VALUES(@subscriptionId, @userId, @productRoleId);
		END
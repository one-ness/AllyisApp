CREATE PROCEDURE [Billing].[UpdateSubscriptionUserProductRole]
	@ProductRoleId INT,
	@SubscriptionId INT,
	@UserId INT
AS
	UPDATE [Billing].[SubscriptionUser] 
		SET [ProductRoleId] = @ProductRoleId
			WHERE [SubscriptionId] = @SubscriptionId AND [UserId] = @UserId;
	IF @@ROWCOUNT=0
		BEGIN
			INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId]) 
			VALUES(@SubscriptionId, @UserId, @ProductRoleId);
		END
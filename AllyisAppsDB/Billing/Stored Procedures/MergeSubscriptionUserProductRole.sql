CREATE PROCEDURE [Billing].[MergeSubscriptionUserProductRole]
	@productRoleId INT,
	@subscriptionId INT,
	@userId INT
AS
	MERGE [Billing].[SubscriptionUser] WITH(HOLDLOCK) AS [T]
	USING (SELECT [ProductRoleId] = @productRoleId, [SubscriptionId] = @subscriptionId, [UserId] = @userId) AS [S]
	ON [T].[SubscriptionId] = [S].[SubscriptionId] AND [T].[UserId] = [S].[UserId]
	WHEN MATCHED THEN
		UPDATE SET [ProductRoleId] = @productRoleId
	WHEN NOT MATCHED THEN
		INSERT ([SubscriptionId], [UserId], [ProductRoleId]) 
		VALUES (@subscriptionId, @userId, @productRoleId);
CREATE PROCEDURE [Billing].[UpdateSubscriptionUserRoles]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@productRoleId INT,
	@productId INT
AS
BEGIN TRANSACTION
	-- get the subscription id from the productId and orgId provided
	DECLARE @subscriptionId INT;
	SELECT
		@subscriptionId = [Subscription].[SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId] -- @productId
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Product].[ProductId] = @productId AND [Subscription].[IsActive] = 1

	-- Update users already in subscription
	UPDATE [Billing].[SubscriptionUser] 
	SET [ProductRoleId] = @productRoleId
	WHERE [SubscriptionId] = @subscriptionId AND [UserId] IN (SELECT [userId] FROM @userIds)

	-- return updated users count
	SELECT @@rOWCOUNT

	-- Select users from @userIds that are not already subscribed
	DECLARE @addingUsers TABLE ([userId] INT);
	INSERT INTO @addingUsers ([userId])
		SELECT [UID].[userId]
		FROM @userIds AS [UID] LEFT OUTER JOIN (
			SELECT [SubscriptionId], [UserId]
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
			WHERE [SubscriptionId] = @subscriptionId)
				[SubUsers] ON [SubUsers].[UserId] = [UID].[userId]
		WHERE [SubscriptionId] IS NULL
			
	-- Add the new users
	DECLARE @subAndRole TABLE ([SubId] INT, [TTRoleId] INT);
	INSERT INTO @subAndRole ([SubId], [TTRoleId]) VALUES (@subscriptionId, @productRoleId)

	INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId])
	SELECT [SubId], [userId], [TTRoleId]
	FROM @addingUsers CROSS JOIN @subAndRole

	-- return added users count
	SELECT COUNT(*) FROM @addingUsers
COMMIT TRANSACTION

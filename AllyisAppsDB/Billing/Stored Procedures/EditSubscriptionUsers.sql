CREATE PROCEDURE [Billing].[EditSubscriptionUsers]
	@OrganizationId INT,
	@UserIds [Auth].[UserTable] READONLY,
	@ProductRoleId INT,
	@ProductId INT
AS
BEGIN TRANSACTION
	DECLARE @SubId INT;
	DECLARE @NumberOfUsers INT;
	SELECT
		@SubId = [SubscriptionId],
		@NumberOfUsers = [NumberOfUsers]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId] -- @productId
	WHERE [Subscription].[OrganizationId] = @OrganizationId AND [Product].[ProductId] = @ProductId AND [Subscription].[IsActive] = 1

	IF @SubId IS NOT NULL
	BEGIN
		IF @ProductRoleId = -1 -- Removing users from subscription
		BEGIN
			DELETE [Billing].[SubscriptionUser] 
			WHERE [SubscriptionId] = @SubId AND [UserId] IN (
				SELECT [userId] FROM @UserIds
			)

			SELECT @@ROWCOUNT
		END
		IF @ProductRoleId > 0 -- Adding/updating subscription users
		BEGIN
			-- Update users already in subscription that are changing roles
			UPDATE [Billing].[SubscriptionUser] 
			SET [ProductRoleId] = @ProductRoleId
			WHERE [SubscriptionId] = @SubId AND [UserId] IN (
				SELECT [userId] FROM @UserIds
			)

			SELECT @@ROWCOUNT

			-- Get users being added to subscription
			DECLARE @AddingUsers TABLE (
				[userId] INT
			);
			INSERT INTO @AddingUsers ([userId])
			SELECT [UID].[userId]
			FROM @UserIds AS [UID]
			LEFT OUTER JOIN (
				SELECT [SubscriptionId], [UserId]
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
				WHERE [SubscriptionId] = @SubId
			) [SubUsers] ON [SubUsers].[UserId] = [UID].[userId]
			WHERE [SubscriptionId] IS NULL
			
			-- Add the new users
			BEGIN
				DECLARE @SubAndRole TABLE ([SubId] INT, [TTRoleId] INT);
				INSERT INTO @SubAndRole ([SubId], [TTRoleId]) VALUES (@SubId, @ProductRoleId)

				INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId])
				SELECT [SubId], [userId], [TTRoleId]
				FROM @AddingUsers
				CROSS JOIN @SubAndRole

				SELECT COUNT(*) FROM @AddingUsers
			END

			--DECLARE @ChangingRoles TABLE (
			--	[UserId] INT
			--);
			--INSERT INTO @ChangingRoles ([UserId])
			--SELECT @UserIds.[userId]
			--FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
			--INNER JOIN @UserIds ON [SubscriptionUser].UserId = @UserIds.[userId]
			--WHERE [SubscriptionId] = @SubId AND [ProductRoleId] != @TimeTrackerRole
		END
	END
	ELSE
	BEGIN	
		SELECT -1 -- No subscription to time tracker
	END
COMMIT TRANSACTION

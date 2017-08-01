/*This query will change the Sku for an organization's subscription and will prevent multiple Skus of the same product*/
CREATE PROCEDURE [Billing].[UpdateSubscription]
	@OrganizationId INT,
	@SkuId INT,
	@ProductId INT,/*Leave this null unless you are trying to delete something (unsubscribe)*/
	@SubscriptionName NVARCHAR(50), 
	@retId INT OUTPUT
AS
	SET NOCOUNT ON;
IF(@SkuId = 0)
	BEGIN
		UPDATE [Billing].[Subscription] SET [Subscription].[IsActive] = 0
			 WHERE [OrganizationId] = @OrganizationId
			 AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
								WHERE [ProductId] = @ProductId);
		DELETE [Billing].[SubscriptionUser]
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId])
					= 0
		UPDATE [Auth].[User] SET [User].[LastSubscriptionId] = NULL
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [User].[LastSubscriptionId])
					= 0
		--Delete from [Billing].[Subscription] where OrganizationId=@OrganizationId and SkuId in 
		--	(select SkuId from Billing.Sku where  ProductId=@productId);
		SET @retId = 0;
	END
ELSE
	BEGIN
		--Find existing subscription by given org that has the same SkuId, if found do nothing and return 0
		IF EXISTS (
			SELECT * FROM [Billing].[Subscription] 
			WHERE [SkuId] = @SkuId AND [OrganizationId] = @OrganizationId AND [IsActive] = 1
		)
		BEGIN
			SET @retId = 0;
		END
		ELSE

		--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
		--Because the productRoleId and subscriptionId don't change so no need to update SubscriptionUser table
		UPDATE [Billing].[Subscription] SET [SkuId] = @SkuId, [SubscriptionName] = @SubscriptionName
			WHERE [OrganizationId] = @OrganizationId
			AND [Subscription].[IsActive] = 1
			AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
							WHERE [SkuId] != @SkuId
							AND [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @SkuId)
							AND [OrganizationId] = @OrganizationId);

		--If not exist, create new subscription and add all org members to the new subscription as sub users
		IF(@@ROWCOUNT=0)
			BEGIN
				--Create the new subscription
				INSERT INTO [Billing].[Subscription] ([OrganizationId], [SkuId], [SubscriptionName])
				VALUES (@OrganizationId, @SkuId, @SubscriptionName);
				SET @retId = SCOPE_IDENTITY();		

				DECLARE @OrgMemberTable TABLE (userId INT) 
				DECLARE @UserProductRoleId INT

				--Find the productId of the given sku
				SELECT @ProductId = [ProductId]
				FROM [Billing].[Sku]
				WHERE [SkuId] = @SkuId

				--Find the ProductRoleId of the User role for the given Product
				SELECT @UserProductRoleId = [ProductRoleId]
				FROM [Auth].[ProductRole]
				WHERE ([ProductId] = @ProductId AND [Name] = 'User')

				--Insert all members of given org to SubscriptionUser table with User role
				INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
				SELECT [UserId], @retId, @UserProductRoleId FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @OrganizationId;
			END
		ELSE
			SET @retId = 0;
	END
/*This query will change the Sku for an organization's subscription and will prevent multiple Skus of the same product*/
CREATE PROCEDURE [Billing].[UpdateSubscription]
	@organizationId INT,
	@skuId INT,
	@productId INT,/*Leave this null unless you are trying to delete something (unsubscribe)*/
	@subscriptionName NVARCHAR(50), 
	@retId INT OUTPUT
AS
	SET NOCOUNT ON;
IF(@skuId = 0)
	BEGIN
		UPDATE [Billing].[Subscription] SET [Subscription].[IsActive] = 0
			 WHERE [OrganizationId] = @organizationId
			 AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
								WHERE [ProductId] = @productId);
		DELETE [Billing].[SubscriptionUser]
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId])
					= 0
		UPDATE [Auth].[User] SET [User].[LastUsedSubscriptionId] = NULL
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [User].[LastUsedSubscriptionId])
					= 0
		--Delete from [Billing].[Subscription] where OrganizationId=@organizationId and SkuId in 
		--	(select SkuId from Billing.Sku where  ProductId=@productId);
		SET @retId = 0;
	END
ELSE
	BEGIN
		--Find existing subscription by given org that has the same SkuId, if found do nothing and return 0
		IF EXISTS (
			SELECT * FROM [Billing].[Subscription] 
			WHERE [SkuId] = @skuId AND [OrganizationId] = @organizationId AND [IsActive] = 1
		)
		BEGIN
			SET @retId = 0;
		END
		ELSE

		--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
		--Because the productRoleId and subscriptionId don't change so no need to update SubscriptionUser table
		UPDATE [Billing].[Subscription] SET [SkuId] = @skuId, [SubscriptionName] = @subscriptionName
			WHERE [OrganizationId] = @organizationId
			AND [Subscription].[IsActive] = 1
			AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
							WHERE [SkuId] != @skuId
							AND [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @skuId)
							AND [OrganizationId] = @organizationId);

		--If not exist, create new subscription and add all org members to the new subscription as sub users
		IF(@@rOWCOUNT=0)
			BEGIN
				--Create the new subscription
				INSERT INTO [Billing].[Subscription] ([OrganizationId], [SkuId], [SubscriptionName])
				VALUES (@organizationId, @skuId, @subscriptionName);
				SET @retId = SCOPE_IDENTITY();		

				DECLARE @orgMemberTable TABLE (userId INT) 
				DECLARE @userProductRoleId INT

				--Find the productId of the given sku
				SELECT @productId = [ProductId]
				FROM [Billing].[Sku]
				WHERE [SkuId] = @skuId

				--Find the ProductRoleId of the User role for the given Product
				SELECT @userProductRoleId = [ProductRoleId]
				FROM [Auth].[ProductRole]
				WHERE ([ProductId] = @productId AND [ProductRoleName] = 'User')

				--Insert all members of given org to SubscriptionUser table with User role
				INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
				SELECT [UserId], @retId, @userProductRoleId FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @organizationId;
			END
		ELSE
			SET @retId = 0;
	END
/*This query will change the Sku for an organization's subscription and will prevent multiple Skus of the same product*/
CREATE PROCEDURE [Billing].[UpdateSubscription]
	@OrganizationId INT,
	@SkuId INT,
	@ProductId INT,/*Leave this null unless you are trying to delete something (unsubscribe)*/
	@NumberOfUsers INT,
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
		UPDATE [Billing].[Subscription] SET [SkuId] = @SkuId, [NumberOfUsers] = @NumberOfUsers
			WHERE [OrganizationId] = @OrganizationId
			AND [Subscription].[IsActive] = 1
			AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
							WHERE [SkuId] != @SkuId
							AND [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @SkuId)
							AND [OrganizationId] = @OrganizationId);
		IF(@@ROWCOUNT=0)
			BEGIN
				INSERT INTO [Billing].[Subscription] ([OrganizationId], [SkuId], [NumberOfUsers])
				VALUES (@OrganizationId, @SkuId, @NumberOfUsers);
				SET @retId = SCOPE_IDENTITY();
			END
		ELSE
			SET @retId = 0;
	END
	
  
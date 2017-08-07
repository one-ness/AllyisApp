CREATE PROCEDURE [Billing].[UpdateSubscriptionName]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50)
AS
	SET NOCOUNT ON;
BEGIN;		
	UPDATE [Billing].[Subscription] SET [SkuId] = @skuId, [SubscriptionName] = @subscriptionName
		WHERE [OrganizationId] = @organizationId
		AND [Subscription].[IsActive] = 1
		AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
						WHERE [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @skuId)
						AND [OrganizationId] = @organizationId);
END;
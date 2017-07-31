CREATE PROCEDURE [Billing].[UpdateSubscriptionName]
	@OrganizationId INT,
	@SkuId INT,
	@NumberOfUsers INT,
	@SubscriptionName NVARCHAR(50)
AS
	SET NOCOUNT ON;
BEGIN;
UPDATE [Billing].[Subscription] SET [SkuId] = @SkuId, [NumberOfUsers] = @NumberOfUsers, [SubscriptionName] = @SubscriptionName
	WHERE [OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
					WHERE [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @SkuId)
					AND [OrganizationId] = @OrganizationId);
END;
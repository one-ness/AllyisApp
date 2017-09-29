-- TODO: pass in subscriptionId as a parameter to simplify logic

CREATE PROCEDURE [Billing].[UpdateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionId INT,
	@subscriptionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
	--Because the productRoleId and subscriptionId don't change, no need to update SubscriptionUser table
	UPDATE [Billing].[Subscription] SET [SkuId] = @skuId, [SubscriptionName] = @subscriptionName
		WHERE [OrganizationId] = @organizationId
		AND [Subscription].[IsActive] = 1
		AND [Subscription].SubscriptionId = @subscriptionId;
END

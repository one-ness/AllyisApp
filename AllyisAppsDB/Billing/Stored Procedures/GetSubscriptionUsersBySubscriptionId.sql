CREATE PROCEDURE [Billing].[GetSubscriptionUsersBySubscriptionId]
	@subscriptionId int
AS
	SELECT 
	[Auth].[User].FirstName,
	[Auth].[User].LastName,
	[Billing].[SubscriptionUser].[ProductRoleId],
	[Auth].[User].UserId,
	[Auth].[User].Email,
	[Billing].[SubscriptionUser].ProductRoleId,
	[Billing].[SubscriptionUser].SubscriptionUserCreatedUtc as 'CreatedUtc',
	[Billing].[SubscriptionUser].[SubscriptionId],
	[Billing].[Sku].ProductId
	FROM 
	[Billing].SubscriptionUser
	JOIN [Auth].[User] ON [Auth].[User].UserId = [SubscriptionUser].UserId
	JOIN [Billing].[Subscription] ON [Billing].[Subscription].SubscriptionId = [Billing].[SubscriptionUser].SubscriptionId
	JOIN [Billing].[Sku] ON [Billing].[Sku].[SkuId] = [Billing].[Subscription].[SkuId]
	WHERE  [SubscriptionUser].SubscriptionId = @subscriptionId;

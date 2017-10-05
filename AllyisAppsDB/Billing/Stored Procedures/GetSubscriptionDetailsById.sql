CREATE PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [OrganizationId]
      ,[Subscription].[SkuId]
	  ,[Subscription].[SubscriptionId]
	  ,[NumberOfUsers]
      ,[SubscriptionCreatedUtc]
      ,[Subscription].[IsActive]
	  ,[SubscriptionName] As 'Name'
	  ,[Sku].[ProductId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
JOIN [Billing].[Sku] ON [Sku].SkuId = [Subscription].[SkuId]
WHERE [SubscriptionId] = @subscriptionId AND [Subscription].[IsActive] = 1
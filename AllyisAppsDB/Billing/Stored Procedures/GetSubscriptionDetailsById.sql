CREATE PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [OrganizationId]
      ,[SkuId]
	  ,[NumberOfUsers]
      ,[SubscriptionCreatedUtc]
      ,[IsActive]
	  ,[SubscriptionName] As 'Name'
FROM [Billing].[Subscription] WITH (NOLOCK) 
WHERE [SubscriptionId] = @subscriptionId AND [IsActive] = 1
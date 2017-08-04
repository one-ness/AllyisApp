CREATE PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [OrganizationId]
      ,[SkuId]
	  ,[NumberOfUsers]
      ,[SubscriptionCreatedUtc]
      ,[IsActive]
FROM [Billing].[Subscription] WITH (NOLOCK) 
WHERE [SubscriptionId] = @SubscriptionId AND [IsActive] = 1
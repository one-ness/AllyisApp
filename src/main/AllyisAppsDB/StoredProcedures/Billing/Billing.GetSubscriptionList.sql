CREATE PROCEDURE [Billing].[GetSubscriptionList]

AS
	SET NOCOUNT ON;

SELECT [SubscriptionId]
	  ,[OrganizationId]
      ,[SkuId]
	  ,[NumberOfUsers]
      ,[CreatedUTC]
      ,[IsActive]
FROM [Billing].[Subscription] WITH (NOLOCK) 

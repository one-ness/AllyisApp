CREATE PROCEDURE [Billing].[GetSkuById]
	@skuId INT
AS
	SET NOCOUNT ON;
	SELECT [SkuId]
      ,[ProductId]
      ,[SkuName]
      ,[CostPerBlock]
      ,[UserLimit]
      ,[BillingFrequency]
	  ,[BlockBasedOn]
      ,[BlockSize]
	  ,[Description]
      ,[PromoCostPerBlock]
      ,[PromoDeadline]
      ,[IsActive]
	  ,[IconUrl]
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] = @skuId
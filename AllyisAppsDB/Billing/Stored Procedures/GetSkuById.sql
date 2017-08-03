CREATE PROCEDURE [Billing].[GetSkuById]
	@SkuId INT
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
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] = @SkuId
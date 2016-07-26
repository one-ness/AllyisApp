CREATE PROCEDURE [Billing].[GetSkuforProduct]
	@ProductId INT
AS
	SET NOCOUNT ON;
	SELECT [SkuId]
      ,[ProductId]
      ,[Name]
      ,[CostPerBlock]
      ,[UserLimit]
      ,[BillingFrequency]
      ,[Tier]
      ,[EntityName]
      ,[BlockSize]
      ,[PromoCostPerBlock]
      ,[PromoDeadline]
      ,[IsActive]
  FROM [Billing].[Sku] WITH (NOLOCK) 
  WHERE [Billing].[Sku].[ProductId] = @ProductId

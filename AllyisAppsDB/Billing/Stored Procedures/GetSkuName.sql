CREATE PROCEDURE [Billing].[GetSkuName]
	@SkuId INT,
	@ProductId INT
AS
	SET NOCOUNT ON;
	SELECT [Name]
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] != @SkuId AND [ProductId] = @ProductId
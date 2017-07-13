CREATE PROCEDURE [Billing].[GetSkuForSubEditById]
	@SkuId INT,
	@ProductId INT
AS
	SET NOCOUNT ON;
	SELECT [SkuId]
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] != @SkuId AND [ProductId] = @ProductId
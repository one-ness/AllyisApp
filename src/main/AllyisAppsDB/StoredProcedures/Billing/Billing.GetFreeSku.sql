CREATE PROCEDURE [Billing].[GetFreeSku]
	@ProductId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[SkuId], 
		[ProductId], 
		[Name], 
		[CostPerBlock], 
		[UserLimit], 
		[BillingFrequency] 
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [ProductId] = @ProductId AND [BillingFrequency] = 'Free'

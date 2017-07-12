CREATE PROCEDURE [Billing].[GetAllActiveProductsAndSkus]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[Name],
		[Product].[Description],
		[Product].[AreaUrl]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1
	ORDER BY [Product].[ProductId]

	SELECT
		[Product].[ProductId],
		[Sku].[SkuId],
		[Sku].[Name],
		[Sku].[CostPerBlock] AS 'Price',
		[Sku].[UserLimit],
		[Sku].[BillingFrequency],
		[Sku].[BlockBasedOn],
		[Sku].[BlockSize],
		[Sku].[Description],
		[Sku].[PromoCostPerBlock],
		[Sku].[PromoDeadline]
	FROM [Billing].[Product] 
	LEFT JOIN [Billing].[Sku]
	WITH (NOLOCK) 
	ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE ([Product].[IsActive] = 1 AND [Sku].[IsActive] = 1)
	ORDER BY [Product].[ProductId]
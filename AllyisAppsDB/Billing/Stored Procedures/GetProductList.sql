CREATE PROCEDURE [Billing].[GetProductList]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[ProductName],
		[Product].[Description]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1
	ORDER BY [Product].[ProductName]
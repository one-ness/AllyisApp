CREATE PROCEDURE [Billing].[GetProductList]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[Name],
		[Product].[Description]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1;

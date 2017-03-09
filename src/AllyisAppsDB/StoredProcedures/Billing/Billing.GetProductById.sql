CREATE PROCEDURE [Billing].[GetProductById]
	@ProductId INT 
AS
	SET NOCOUNT ON;
	SELECT [Product].[Name], [Product].[ProductId], [Product].[Description]
	  FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductId] = @ProductId
CREATE PROCEDURE [Billing].[GetProductById]
	@ProductId INT 
AS
	SET NOCOUNT ON;
	SELECT [Product].[Name], [Product].[ProductId], [Product].[Description], [Product].[AreaUrl]
	  FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductId] = @ProductId
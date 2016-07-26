CREATE PROCEDURE [Billing].[GetProductIds]
AS
	SET NOCOUNT ON;
	SELECT [ProductId] FROM [Billing].[Product] WITH (NOLOCK) ;

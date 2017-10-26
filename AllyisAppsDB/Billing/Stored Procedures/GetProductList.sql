CREATE PROCEDURE [Billing].[GetProductList]
AS
	SET NOCOUNT ON;
	SELECT * from Product with (nolock)
	order by ProductName asc
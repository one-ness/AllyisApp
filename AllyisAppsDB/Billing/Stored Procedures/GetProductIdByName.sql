CREATE PROCEDURE [Billing].[GetProductIdByName]
	@productName NVARCHAR(128)
AS
	SET NOCOUNT ON;
	SELECT [ProductId] FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductName] = @productName;
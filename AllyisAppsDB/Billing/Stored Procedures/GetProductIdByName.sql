CREATE PROCEDURE [Billing].[GetProductIdByName]
	@ProductName NVARCHAR(128)
AS
	SET NOCOUNT ON;
	SELECT [ProductId] FROM [Billing].[Product] WITH (NOLOCK) WHERE [Name] = @ProductName;
CREATE PROCEDURE [Crm].[ReactivateCustomer]
	@customerId INT
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the customer's name
	DECLARE @CustomerName NVARCHAR(384);

	SELECT 
		@CustomerName = [CustomerName] 
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [CustomerId] = @customerId

	IF @CustomerName IS NOT NULL
	BEGIN --Customer found
		UPDATE [Crm].[Customer] SET [IsActive] = 1
		WHERE [Customer].[CustomerId] = @customerId;
	END
	SELECT @CustomerName
END

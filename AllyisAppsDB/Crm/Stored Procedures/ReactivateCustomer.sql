CREATE PROCEDURE [Crm].[ReactivateCustomer]
	@customerId INT
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the customer's name
	DECLARE @customerName NVARCHAR(384);

	SELECT 
		@customerName = [CustomerName] 
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [CustomerId] = @customerId

	IF @customerName IS NOT NULL
	BEGIN --Customer found
		UPDATE [Crm].[Customer] SET [IsActive] = 1
		WHERE [Customer].[CustomerId] = @customerId;
	END
	SELECT @customerName
END

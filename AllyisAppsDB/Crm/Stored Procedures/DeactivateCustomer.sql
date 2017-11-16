CREATE PROCEDURE [Crm].[DeactivateCustomer]
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
		UPDATE [Crm].[Customer] SET [IsActive] = 0
		WHERE [Customer].[CustomerId] = @customerId;
	 
		UPDATE [Pjm].[Project] SET [IsActive] = 0
		WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [IsActive] = 0);
	END
	SELECT @customerName
END
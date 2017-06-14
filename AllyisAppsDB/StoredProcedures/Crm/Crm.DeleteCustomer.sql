CREATE PROCEDURE [Crm].[DeleteCustomer]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the customer's name
	DECLARE @CustomerName NVARCHAR(384);

	SELECT 
		@CustomerName = [Name] 
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [CustomerId] = @CustomerId

	IF @CustomerName IS NOT NULL
	BEGIN --Customer found
		UPDATE [Crm].[Customer] SET [IsActive] = 0
		WHERE [Customer].[CustomerId] = @CustomerId;
	 
		UPDATE [Crm].[Project] SET [IsActive] = 0
		WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [IsActive] = 0);
	END
	SELECT @CustomerName
END

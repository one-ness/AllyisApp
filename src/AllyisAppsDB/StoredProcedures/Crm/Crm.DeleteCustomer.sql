CREATE PROCEDURE [Crm].[DeleteCustomer]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Crm].[Customer] SET [IsActive] = 0
	WHERE [Customer].[CustomerId] = @CustomerId;
	 
	UPDATE [Crm].[Project] SET [IsActive] = 0
	WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [IsActive] = 0);
END

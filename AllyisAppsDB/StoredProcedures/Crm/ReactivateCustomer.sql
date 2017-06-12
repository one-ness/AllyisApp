CREATE PROCEDURE [Crm].[ReactivateCustomer]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Crm].[Customer] SET [IsActive] = 1
	WHERE [Customer].[CustomerId] = @CustomerId;
	 
	-- There shouldnt be a need for the following procedure since
	-- projects should be deactivated before a customer is deactivated
	/*
	UPDATE [Crm].[Project] SET [IsActive] = 1
	WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [IsActive] = 0);
	*/
END
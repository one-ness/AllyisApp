CREATE PROCEDURE [Crm].[FullDeleteCustomer]
	@customerId INT
AS
	DELETE FROM [Crm].[Customer] WHERE [CustomerId] = @customerId
RETURN 1


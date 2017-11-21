CREATE PROCEDURE [Crm].[UpdateCustomerIsActive]
    @customerId INT,
    @isActive BIT
AS

SET NOCOUNT ON;

UPDATE [Crm].[Customer]
   SET [IsActive] = @isActive
 WHERE [CustomerId] = @customerId

SELECT [CustomerName]
  FROM [Crm].[Customer] WITH (NOLOCK)
 WHERE [CustomerId] = @customerId

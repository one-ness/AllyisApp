CREATE PROCEDURE [Crm].[GetCustomerInfo]
	@customerId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Customer].[AddressId],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerCode],
		   [Customer].[IsActive],
		   [Customer].[ActiveProjectCount],
		   [Customer].[ProjectCount]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	WHERE [CustomerId] = @customerId
END
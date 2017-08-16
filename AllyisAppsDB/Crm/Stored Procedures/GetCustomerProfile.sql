CREATE PROCEDURE [Crm].[GetCustomerProfile]
	@customerId INT
AS
BEGIN

	DECLARE @addressId AS INT
	
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
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	WHERE [CustomerId] = @customerId

	SET @addressId = (SELECT m.AddressId
					FROM [Crm].[Customer] AS m
					WHERE [CustomerId] = @customerId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Address].[CountryCode],
		   [Address].[StateId],
		   [Country].[CountryName] AS 'Country',
		   [Address].[PostalCode],
		   [Address].[AddressId]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @addressId
END
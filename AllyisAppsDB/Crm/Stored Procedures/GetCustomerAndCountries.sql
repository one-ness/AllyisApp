CREATE PROCEDURE [Crm].[GetCustomerAndCountries]
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
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [CustomerId] = @customerId
	
	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK);

	SET @addressId = (SELECT m.AddressId
					FROM [Crm].[Customer] AS m
					WHERE [CustomerId] = @customerId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @addressId
END
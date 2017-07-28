CREATE PROCEDURE [Crm].[GetCustomerAndCountries]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Customer].[AddressId],
		   [Address].[Address1] AS 'Address',
		   [Address].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [Address].[PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CreatedUtc],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE [CustomerId] = @CustomerId
	
	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK) ;
END
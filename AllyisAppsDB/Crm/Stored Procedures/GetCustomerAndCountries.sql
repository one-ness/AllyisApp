CREATE PROCEDURE [Crm].[GetCustomerAndCountries]
	@CustomerId INT
AS
BEGIN

	DECLARE @AddressId AS INT
	
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Customer].[AddressId],
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
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [CustomerId] = @CustomerId
	
	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK);

	SET @AddressId = (SELECT m.AddressId
					FROM [Crm].[Customer] AS m
					WHERE [CustomerId] = @CustomerId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @AddressId
END
CREATE PROCEDURE [Crm].[GetCustomersByOrgId]
	@orgId INT
AS
	BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @orgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[CustomerName]
END
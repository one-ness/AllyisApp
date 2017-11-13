CREATE PROCEDURE [Crm].[GetCustomersByOrgId]
	@orgId INT
AS
	BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'StateName',
		   [State].[StateId],
		   [Country].[CountryName] AS 'CountryName',
		   [Country].[CountryCode],
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerOrgId],
		   [Customer].[AddressId],
		   [Customer].[IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @orgId
	ORDER BY [Customer].[CustomerName]
END
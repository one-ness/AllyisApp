CREATE PROCEDURE [Crm].[GetCustomersByOrgId]
	@OrgId INT
AS
	BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Address1] AS 'Address',
		   [City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[Name]
END
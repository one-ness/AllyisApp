CREATE PROCEDURE [Crm].[GetCustomersByOrgId]
	@OrgId INT
AS
	BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Customer].[Address],
		   [Customer].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [Customer].[PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CreatedUTC],
		   [Customer],[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Customer].[Country]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Customer].[State]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[Name]
END 

CREATE PROCEDURE [Crm].[GetNextCustIdAndCountries]
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[CustomerOrgId]
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @orgId
	ORDER BY [CustomerOrgId] DESC
	
	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK) ;
END
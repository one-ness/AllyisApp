CREATE PROCEDURE [Crm].[GetNextCustIdAndCountries]
	@OrgId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[CustomerOrgId]
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @OrgId
	ORDER BY [CustomerOrgId] DESC
	
	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK) ;
END

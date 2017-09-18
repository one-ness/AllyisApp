CREATE PROCEDURE [Auth].[GetOrg]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl], 
		[Address1] AS 'Address',
		[Organization].[AddressId],
		[City], 
		[State].[StateName] AS 'StateName', 
		[Country].[CountryName] AS 'CountryName', 
		[State].[StateId],
		[Country].[CountryCode],
		[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @organizationId
END
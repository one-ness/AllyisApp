CREATE PROCEDURE [Auth].[GetOrgWithCountriesAndEmployeeId]
	@organizationId int,
	@userId int
AS
	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl], 
		[Organization].[AddressId],
		[Address].[Address1] AS 'Address',
		[Address].[City], 
		[State].[StateName] AS 'State', 
		[Country].[CountryName] AS 'Country', 
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @organizationId

	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK)

	SELECT [EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @organizationId AND [OrganizationUser].[UserId] = @userId
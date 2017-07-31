CREATE PROCEDURE [Auth].[GetOrgWithCountriesAndEmployeeId]
	@OrganizationId int,
	@UserId int
AS
	SELECT 
		[OrganizationId],
		[Organization].[Name],
		[SiteUrl], 
		[Organization].[AddressId],
		[Address].[Address1] AS 'Address',
		[Address].[City], 
		[State].[Name] AS 'State', 
		[Country].[Name] AS 'Country', 
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[CreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE OrganizationId = @OrganizationId

	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK)

	SELECT [EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId AND [OrganizationUser].[UserId] = @UserId
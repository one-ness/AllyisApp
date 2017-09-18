CREATE PROCEDURE [Auth].[GetOrgWithNextEmployeeId]
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
		[State].[StateName] AS 'StateName', 
		[Country].[CountryName] AS 'CountryName', 
		[State].[StateId],
		[Country].[CountryCode] AS 'CountryCode',
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc] AS 'CreatedUtc'

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @organizationId


	SELECT [EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @organizationId AND [OrganizationUser].[UserId] = @userId
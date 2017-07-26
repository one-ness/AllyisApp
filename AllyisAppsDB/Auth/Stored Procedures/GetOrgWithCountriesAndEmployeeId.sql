CREATE PROCEDURE [Auth].[GetOrgWithCountriesAndEmployeeId]
	@OrganizationId int,
	@UserId int
AS
	SELECT 
		[OrganizationId],
		[Organization].[Name],
		[SiteUrl], 
		[Address], 
		[City], 
		[State].[Name] AS 'State', 
		[Country].[Name] AS 'Country', 
		[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[CreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Organization].[Country]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Organization].[State]
	WHERE OrganizationId = @OrganizationId

	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK)

	SELECT [EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId AND [OrganizationUser].[UserId] = @UserId
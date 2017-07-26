CREATE PROCEDURE [Auth].[GetOrg]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

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
END
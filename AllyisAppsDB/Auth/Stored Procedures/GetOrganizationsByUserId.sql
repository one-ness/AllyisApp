CREATE PROCEDURE [Auth].[GetOrganizationsByUserId]
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;
SELECT [Auth].[Organization].[OrganizationId]
      ,[Organization].[OrganizationName]
      ,[SiteUrl]
      ,[Address1] AS 'Address'
      ,[City]
      ,[State].[StateName] AS 'State'
      ,[Country].[CountryName] AS 'Country'
      ,[PostalCode]
      ,[PhoneNumber]
	  ,[FaxNumber]
      ,[Organization].[OrganizationCreatedUtc]
FROM [Auth].[Organization] WITH (NOLOCK)
RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
JOIN [Lookup].[Address]					WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
WHERE [OrganizationUser].[UserId] = @userId 
      AND [Auth].[Organization].[IsActive] = 1
ORDER BY [OrganizationUser].[OrganizationRoleId] DESC, [Organization].[OrganizationName]
END
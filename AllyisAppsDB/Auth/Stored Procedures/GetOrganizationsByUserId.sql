CREATE PROCEDURE [Auth].[GetOrganizationsByUserId]
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;
SELECT [Auth].[Organization].[OrganizationId]
      ,[Organization].[OrganizationName]
      ,[SiteUrl]
      ,[Address1] AS 'Address'
	  ,[Organization].[AddressId]
      ,[City]
      ,[State].[StateName] AS 'StateName'
	  ,[State].[StateId]
      ,[Country].[CountryName] AS 'CountryName'
	  ,[Country].[CountryCode] AS 'CountryCode'
      ,[PostalCode]
      ,[PhoneNumber]
	  ,[FaxNumber]
      ,[Organization].[OrganizationCreatedUtc]
FROM [Auth].[Organization] WITH (NOLOCK)
RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
JOIN [Lookup].[Address]					WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
WHERE [OrganizationUser].[UserId] = @userId 
      AND [Auth].[Organization].[IsActive] = 1
ORDER BY [OrganizationUser].[OrganizationRoleId] DESC, [Organization].[OrganizationName]
END
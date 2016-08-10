CREATE PROCEDURE [Auth].[GetOrganizationsByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
SELECT [Auth].[Organization].[OrganizationId]
      ,[Organization].[Name]
      ,[SiteUrl]
      ,[Address]
      ,[City]
      ,[State].[Name] AS 'State'
      ,[Country].[Name] AS 'Country'
      ,[PostalCode]
      ,[PhoneNumber]
	  ,[FaxNumber]
      ,[Organization].[CreatedUTC]
FROM [Auth].[Organization] WITH (NOLOCK)
RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryId] = [Organization].[Country]
LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Organization].[State]
WHERE [OrganizationUser].[UserId] = @UserId 
      AND [Auth].[Organization].[IsActive] = 1
ORDER BY [OrganizationUser].[OrgRoleId] DESC, [Organization].[Name]
END
CREATE PROCEDURE [Lookup].[GetStatesByCountry]
	@countryName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF ((SELECT Count(*) FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @countryName) <> 0)

			SELECT [State].[StateName]
			FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @countryName
			ORDER BY [State].[StateName]
		
	ELSE
		SELECT @countryName
			 
END
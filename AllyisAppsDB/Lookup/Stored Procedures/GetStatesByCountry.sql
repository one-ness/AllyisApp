CREATE PROCEDURE [Lookup].[GetStatesByCountry]
	@CountryName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF ((SELECT Count(*) FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[Name] = @CountryName) <> 0)

			SELECT [State].[Name]
			FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[Name] = @CountryName
			ORDER BY [State].[Name]
		
	ELSE
		SELECT @CountryName
			 
END
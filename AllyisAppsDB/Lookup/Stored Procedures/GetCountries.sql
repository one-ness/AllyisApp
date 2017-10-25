CREATE PROCEDURE [Lookup].[GetCountries]
AS
	SET NOCOUNT ON;
	SELECT * FROM [Lookup].[Country] WITH (NOLOCK)
	order by CountryName asc
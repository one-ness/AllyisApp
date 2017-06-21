CREATE PROCEDURE [Lookup].[GetCountries]
AS
	SET NOCOUNT ON;
	SELECT [Name] FROM [Lookup].[Country] WITH (NOLOCK) ;
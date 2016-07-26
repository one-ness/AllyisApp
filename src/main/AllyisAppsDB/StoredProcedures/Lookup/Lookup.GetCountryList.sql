CREATE PROCEDURE [Lookup].[GetCountryList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [CountryId], [Name], [Code] FROM Country WITH (NOLOCK)
END

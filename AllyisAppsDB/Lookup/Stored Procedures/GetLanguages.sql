CREATE PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[id] AS [LanguageID],
		[LanguageName],
		[CultureName]
	FROM [Language] WITH (NOLOCK)
END
CREATE PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[Id] AS [LanguageID],
		[LanguageName],
		[CultureName]
	FROM [Language] WITH (NOLOCK)
END
CREATE PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[Id] AS [LanguageId],
		[LanguageName],
		[CultureName]
	FROM [Language] WITH (NOLOCK)
END
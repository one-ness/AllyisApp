CREATE PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[CultureName],
		[LanguageName]
	FROM [Language] WITH (NOLOCK)
END
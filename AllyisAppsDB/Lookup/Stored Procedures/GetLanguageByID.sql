CREATE PROCEDURE [Lookup].[GetLanguageById]
	@CultureName VARCHAR (16)
AS
BEGIN
	SELECT
		[Language].[LanguageName],
		[Language].[CultureName]
	FROM [Lookup].[Language] WITH (NOLOCK)
	WHERE [Language].[CultureName] = @CultureName
	 
END
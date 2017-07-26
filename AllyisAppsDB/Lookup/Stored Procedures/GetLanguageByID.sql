CREATE PROCEDURE [Lookup].[GetLanguageById]
	@LanguageId INT
AS
BEGIN
	SELECT
		[Language].[Id] AS [LanguageId],
		[Language].[LanguageName],
		[Language].[CultureName]
	FROM [Lookup].[Language] WITH (NOLOCK)
	WHERE [Language].[Id] = @LanguageId
	 
END
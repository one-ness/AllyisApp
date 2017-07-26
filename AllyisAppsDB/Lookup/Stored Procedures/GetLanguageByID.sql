CREATE PROCEDURE [Lookup].[GetLanguageByID]
	@LanguageID INT
AS
BEGIN
	SELECT
		[Language].[Id] AS [LanguageID],
		[Language].[LanguageName],
		[Language].[CultureName]
	FROM [Lookup].[Language] WITH (NOLOCK)
	WHERE [Language].[Id] = @LanguageID
	 
END
CREATE PROCEDURE [Lookup].[GetLanguageByID]
	@LanguageID INT
AS
BEGIN
	SELECT
		[Language].[id] AS [LanguageID],
		[Language].[LanguageName],
		[Language].[CultureName]
	FROM [Lookup].[Language] WITH (NOLOCK)
	WHERE [Language].[id] = @LanguageID
	 
END
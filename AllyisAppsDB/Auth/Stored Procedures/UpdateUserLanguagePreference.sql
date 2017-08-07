CREATE PROCEDURE [Auth].[UpdateUserLanguagePreference]
	@id INT,
	@languageId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [PreferredLanguageId] = @languageId
	WHERE [UserId] = @id
END

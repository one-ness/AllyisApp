CREATE PROCEDURE [Auth].[UpdateUserLanguagePreference]
	@id INT,
	@CultureName VARCHAR (16)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [PreferredLanguageId] = @CultureName
	WHERE [UserId] = @id
END

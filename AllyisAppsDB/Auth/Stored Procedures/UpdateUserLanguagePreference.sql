CREATE PROCEDURE [Auth].[UpdateUserLanguagePreference]
	@Id INT,
	@LanguageId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [PreferredLanguageId] = @LanguageId
	WHERE [UserId] = @Id
END
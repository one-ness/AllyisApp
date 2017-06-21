CREATE PROCEDURE [Auth].[UpdateUserLanguagePreference]
	@Id INT,
	@LanguageID INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [LanguagePreference] = @LanguageID
	WHERE [UserId] = @Id
END
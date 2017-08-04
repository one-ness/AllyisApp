CREATE PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET [IsEmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode
END

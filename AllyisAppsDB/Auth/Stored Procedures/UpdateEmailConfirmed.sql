CREATE PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
	UPDATE [Auth].[User]
	SET [IsEmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode

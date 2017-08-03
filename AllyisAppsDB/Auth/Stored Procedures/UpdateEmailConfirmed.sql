CREATE PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
BEGIN
	SELECT [UserId]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [EmailConfirmationCode] = @emailConfirmCode and [IsEmailConfirmed] = 0

	UPDATE [Auth].[User]
	SET [IsEmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode
END
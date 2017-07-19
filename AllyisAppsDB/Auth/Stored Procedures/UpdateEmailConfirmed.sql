CREATE PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
BEGIN
	SELECT [UserId]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [EmailConfirmationCode] = @emailConfirmCode and [EmailConfirmed] = 0

	UPDATE [Auth].[User]
	SET [EmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode
END
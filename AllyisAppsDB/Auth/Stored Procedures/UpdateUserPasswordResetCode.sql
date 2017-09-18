CREATE PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@email nvarchar (384),
	@passwordResetCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET PasswordResetCode = @passwordResetCode
	WHERE Email = @email
END

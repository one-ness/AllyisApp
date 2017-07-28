CREATE PROCEDURE [Auth].[UpdateUserPasswordUsingCode]
	@passwordHash nvarchar(512),
	@PasswordResetCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash, [PasswordResetCode] = NULL
	WHERE [PasswordResetCode] = @passwordResetCode

	select @@ROWCOUNT
END
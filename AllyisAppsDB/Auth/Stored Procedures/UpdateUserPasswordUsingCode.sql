CREATE PROCEDURE [Auth].[UpdateUserPasswordUsingCode]
	@passwordHash nvarchar(512),
	@passwordResetCode uniqueidentifier
AS
BEGIN
	set nocount on
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash, [PasswordResetCode] = NULL
	WHERE [PasswordResetCode] = @passwordResetCode
	select @@ROWCOUNT
END

CREATE PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@UserId nvarchar (40) ,
	@PasswordResetCode nvarchar (MAX)
AS
BEGIN
	UPDATE [Auth].[User]
	SET [PasswordResetCode] = @PasswordResetCode
	WHERE [UserId] = @UserId
END

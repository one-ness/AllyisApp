CREATE PROCEDURE [Auth].[UpdateUserPasswordUsingCode]
	@UserId nvarchar (40) ,
	@PasswordHash nvarchar (MAX),
	@PasswordResetCode nvarchar (MAX)
AS
BEGIN
	SELECT [UserId] FROM [Auth].[User]
	WHERE [UserId] = @UserId AND [PasswordResetCode] = @PasswordResetCode

	UPDATE [Auth].[User]
	SET [PasswordHash] = @PasswordHash, [PasswordResetCode] = NULL
	WHERE [UserId] = @UserId AND [PasswordResetCode] = @PasswordResetCode
END

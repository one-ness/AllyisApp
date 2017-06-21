CREATE PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@Email nvarchar (384) ,
	@PasswordResetCode nvarchar (MAX)
AS
BEGIN
	IF EXISTS (
		SELECT * FROM [Auth].[User]
		WHERE [Email] = @Email
	)
	BEGIN
		UPDATE [Auth].[User]
		SET [PasswordResetCode] = @PasswordResetCode
		WHERE [Email] = @Email

		-- Return user id on success
		SELECT [UserId]
		FROM [Auth].[User] WITH (NOLOCK)
		WHERE [Email] = @Email
	END
	ELSE
	BEGIN
		SELECT -1 --Indicates no account w/ given email
	END
END
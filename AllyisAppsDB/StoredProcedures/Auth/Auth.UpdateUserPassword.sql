CREATE PROCEDURE [Auth].[UpdateUserPassword]
	@UserId nvarchar (40) ,
	@PasswordHash nvarchar (MAX)
AS
BEGIN
	UPDATE [Auth].[User]
	SET [PasswordHash] = @PasswordHash
	WHERE [UserId] = @UserId

	SELECT [PasswordHash]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [UserId] = @UserId
END

CREATE PROCEDURE [Auth].[UpdateUserPassword]
	@userId int,
	@passwordHash nvarchar(512)
AS
BEGIN
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash
	WHERE [UserId] = @userId

	SELECT [PasswordHash]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [UserId] = @userId
END
CREATE PROCEDURE [Auth].[UpdateUserPassword]
	@userId int,
	@passwordHash nvarchar(512)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash
	WHERE [UserId] = @userId
END

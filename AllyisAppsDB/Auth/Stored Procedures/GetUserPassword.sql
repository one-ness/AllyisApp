CREATE PROCEDURE [Auth].[GetUserPassword]
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PasswordHash]
	FROM [Auth].[User]
	WITH (NOLOCK)
	WHERE [UserId] = @userId;
END
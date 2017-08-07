CREATE PROCEDURE [Auth].[GetPasswordHashFromUserId]
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PasswordHash]
	FROM [Auth].[User]
	WITH (NOLOCK)
	WHERE [UserId] = @userId;
END
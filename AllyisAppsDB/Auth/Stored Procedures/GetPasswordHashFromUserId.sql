CREATE PROCEDURE [Auth].[GetPasswordHashFromUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PasswordHash]
	FROM [Auth].[User]
	WITH (NOLOCK)
	WHERE [UserId] = @UserId;
END
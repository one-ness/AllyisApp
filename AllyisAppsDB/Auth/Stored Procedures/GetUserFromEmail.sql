CREATE PROCEDURE [Auth].[GetUserFromEmail]
	@email NVARCHAR(384)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT u.* FROM [Auth].[User] u with (nolock)
	WHERE u.[Email] = @email;
END
CREATE PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@email nvarchar (384),
	@passwordResetCode uniqueidentifier
AS
BEGIN
	set nocount on
	UPDATE [Auth].[User]
	SET PasswordResetCode = @passwordResetCode
	WHERE Email = @email
	select @@ROWCOUNT
END

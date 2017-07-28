CREATE PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@email nvarchar (384) ,
	@passwordResetCode uniqueidentifier
AS
begin
		UPDATE [User] set PasswordResetCode = @passwordResetCode
		where Email = @email
		select @@ROWCOUNT
end
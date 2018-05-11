create procedure Auth.UpdateUserLoginProvider
	@userId int,
	@loginProviderId int
as
begin
	set nocount on
	update [User] set PasswordHash = null, LoginProviderId = @loginProviderId
	where UserId = @userId
end
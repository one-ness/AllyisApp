create procedure Auth.GetUser2
	@userId int
as
begin
	set nocount on
	select * from Auth.[User] u
	where u.UserId = @userId
end
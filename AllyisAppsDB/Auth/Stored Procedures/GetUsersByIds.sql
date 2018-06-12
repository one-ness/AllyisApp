create procedure Auth.GetUsersByIds
	@csvUserIds nvarchar(max)
as
begin
	set nocount on
	select u.* from [User] u with (nolock)
	inner join dbo.SplitNumberString(@csvUserIds) t1 on t1.Number = u.UserId
end
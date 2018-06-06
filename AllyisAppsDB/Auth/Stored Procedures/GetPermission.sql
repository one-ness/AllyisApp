CREATE procedure Auth.GetPermission
	@productRoleId int,
	@userActionId int,
	@appEntityId int
as
begin
	set nocount on
	select top 1 * from Permission with (nolock)
	where ProductRoleId = @productRoleId and UserActionId = @userActionId and AppEntityId = @appEntityId
end
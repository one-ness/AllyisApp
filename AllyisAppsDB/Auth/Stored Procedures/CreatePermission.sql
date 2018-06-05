CREATE procedure [Auth].[CreatePermission]
	@productRoleId int,
	@userActionId int,
	@actionGroupId int,
	@isDenied bit = 0
as
begin
	set nocount on
	insert into Permission (ProductRoleId, UseractionId, ActionGroupId, IsDenied)
	values (@productRoleId, @userActionId, @actionGroupId, @isDenied)
end
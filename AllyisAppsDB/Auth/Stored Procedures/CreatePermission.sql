CREATE procedure [Auth].[CreatePermission]
	@productRoleId int,
	@userActionId int,
	@appEntityId int,
	@isDenied bit = 0
as
begin
	set nocount on
	insert into Permission (ProductRoleId, UseractionId, AppEntityId, IsDenied)
	values (@productRoleId, @userActionId, @appEntityId, @isDenied)
end
CREATE procedure [Auth].[CreatePermission]
	@productRoleId int,
	@entityId int,
	@actionId int,
	@isAllowed bit
as
begin
	set nocount on
	insert into Permission (ProductRoleId, EntityId, ActionId, IsAllowed)
	values (@productRoleId, @entityId, @actionId, @isAllowed)
end
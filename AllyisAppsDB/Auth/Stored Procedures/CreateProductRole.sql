CREATE procedure Auth.CreateProductRole
	@productId int,
	@productRoleShortName nvarchar(32),
	@productRoleFullName nvarchar(64),
	@orgOrSubId int,
	@builtinProductRoleId int
as
begin
	set nocount on
	insert into ProductRole (ProductId, ProductRoleShortName, ProductRoleFullName, OrgOrSubId, BuiltinProductRoleId)
	values (@productId, @productRoleShortName, @productRoleFullName, @orgOrSubId, @builtinProductRoleId)
	select SCOPE_IDENTITY()
end
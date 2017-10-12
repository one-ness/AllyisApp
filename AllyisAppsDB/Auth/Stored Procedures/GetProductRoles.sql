create procedure Auth.GetProductRoles
	@orgId int,
	@productId int
as
begin
	set nocount on
	-- NOTE: IGNORE orgId for now, but later we need to use it
	select * from ProductRole with (nolock)
	where ProductId = @productId
end
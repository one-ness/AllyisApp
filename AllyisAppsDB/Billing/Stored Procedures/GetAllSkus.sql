create procedure Billing.GetAllSkus
as
begin
	set nocount on
	select * from Sku with (nolock)
end
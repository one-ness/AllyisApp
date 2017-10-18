create procedure Billing.UpdateSubscriptionSkuAndName
	@subscriptionId int,
	@subscriptionName nvarchar(64),
	@skuId int
as
begin
	set nocount on
	update Subscription set SubscriptionName = @subscriptionName, SkuId = @skuId
	where SubscriptionId = @subscriptionId
end
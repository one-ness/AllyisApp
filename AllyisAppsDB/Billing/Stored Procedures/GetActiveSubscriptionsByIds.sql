CREATE procedure [Billing].[GetActiveSubscriptionsByIds]
	@csvSubIds nvarchar(max)
as
begin
	set nocount on
	select s.* from Subscription s with (nolock)
	inner join dbo.SplitNumberString(@csvSubIds) t1 on t1.Number = s.SubscriptionId
	--where s.IsActive = 1
end
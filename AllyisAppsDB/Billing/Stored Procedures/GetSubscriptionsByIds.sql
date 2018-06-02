CREATE procedure [Billing].[GetSubscriptionsByIds]
	@csvSubIds nvarchar(max),
	@statusMask int
as
begin
	set nocount on
	select s.* from Subscription s with (nolock)
	inner join dbo.SplitNumberString(@csvSubIds) t1 on t1.Number = s.SubscriptionId
	where (s.SubscriptionStatus & @statusMask) > 0
end
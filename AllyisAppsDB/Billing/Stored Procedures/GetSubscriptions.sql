CREATE PROCEDURE [Billing].[GetSubscriptions]
    @orgId int,
	@subStatus int
AS
BEGIN
    SET NOCOUNT ON
	select * from Subscription with (nolock)
	where OrganizationId = @orgId and (SubscriptionStatus & @subStatus > 0)
END
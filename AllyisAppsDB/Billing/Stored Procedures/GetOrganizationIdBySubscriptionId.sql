CREATE PROCEDURE [Billing].[GetOrganizationIdBySubscriptionId]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	select [OrganizationId] from Subscription with (nolock)
	where [SubscriptionId] = @subscriptionId
END
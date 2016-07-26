CREATE PROCEDURE [Billing].[GetAllSubscriptionsForOrganization]
	@OrgId INT
AS
	SET NOCOUNT ON;
	SELECT [StripeTokenSubId] 
	FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
	WHERE [StripeCustomerSubscriptionPlan].[OrganizationId] = @OrgId

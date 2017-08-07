CREATE PROCEDURE [Billing].[GetSubscriptionPlanPricesByOrg]
	@orgId INT
AS
	SET NOCOUNT ON;
	SELECT [Price]
	FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1
CREATE PROCEDURE [Billing].[GetSubscriptionPlanPricesByOrg]
	@OrgId INT
AS
	SET NOCOUNT ON;
	SELECT [Price]
	FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrgId AND [IsActive] = 1

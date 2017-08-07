CREATE PROCEDURE [Billing].[GetSubscriptionPlan]
	@customerId NVARCHAR(50), 
	@organizationId INT
AS
	
	SET NOCOUNT ON;
SELECT [StripeCustomerSubscriptionPlan].[StripeTokenSubId]
FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
WHERE [StripeTokenCustId] = @customerId AND [OrganizationId] = @organizationId AND [IsActive] = 1;
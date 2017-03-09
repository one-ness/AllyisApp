CREATE PROCEDURE [Billing].[GetSubscriptionPlan]
	@CustomerId NVARCHAR(50), 
	@OrganizationId INT
AS
	
	SET NOCOUNT ON;
SELECT [StripeCustomerSubscriptionPlan].[StripeTokenSubId]
FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
WHERE [StripeTokenCustId] = @CustomerId AND [OrganizationId] = @OrganizationId AND [IsActive] = 1; 


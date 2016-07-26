CREATE PROCEDURE [Billing].[GetStripeSubscriptionPlanPrice]
	@SubscriptionId NVARCHAR(50)
	
AS
	SET NOCOUNT ON;
SELECT [StripeCustomerSubscriptionPlan].[Price]
FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
WHERE [StripeTokenSubId] = @SubscriptionId AND [IsActive] = 1;  
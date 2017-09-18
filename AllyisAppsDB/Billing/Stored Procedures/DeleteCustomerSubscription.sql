CREATE PROCEDURE [Billing].[DeleteCustomerSubscription]
	@stripeTokenSubId NVARCHAR(50)
AS
	SET NOCOUNT ON;
	Update [Billing].[StripeCustomerSubscriptionPlan]
	SET [IsActive] = 0 
	WHERE [StripeTokenSubId] = @stripeTokenSubId;
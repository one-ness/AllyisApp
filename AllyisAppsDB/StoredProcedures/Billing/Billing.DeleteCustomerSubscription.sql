CREATE PROCEDURE [Billing].[DeleteCustomerSubscription]
	@StripeTokenSubId NVARCHAR(50)
AS
	SET NOCOUNT ON;
	Update [Billing].[StripeCustomerSubscriptionPlan]
	SET [IsActive] = 0 
	WHERE [StripeTokenSubId] = @StripeTokenSubId; 


CREATE PROCEDURE [Billing].[UpdateCustomerSubscription]
	@customerID NVARCHAR(50),
	@SubPlanId NVARCHAR(50),
	@NumberOfUsers INT,
	@Price INT
AS
	SET NOCOUNT ON;
BEGIN
	UPDATE [Billing].[StripeCustomerSubscriptionPlan] 
		SET [StripeCustomerSubscriptionPlan].[NumberOfUsers] = @NumberOfUsers,
		[StripeCustomerSubscriptionPlan].[Price] = @Price
		WHERE [StripeTokenCustId] = @customerID
		AND [StripeTokenSubId] = @SubPlanId;
END
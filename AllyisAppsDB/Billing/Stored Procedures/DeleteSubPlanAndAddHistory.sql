CREATE PROCEDURE [Billing].[DeleteSubPlanAndAddHistory]
	@customerId NVARCHAR(50), 
	@organizationId INT,
	@userId INT,
	@skuId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the stripe token subscription id
	DECLARE @stripeSubId NCHAR(50);
	SELECT @stripeSubId = [StripeCustomerSubscriptionPlan].[StripeTokenSubId]
	FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
	WHERE [StripeTokenCustId] = @customerId AND [OrganizationId] = @organizationId AND [IsActive] = 1;

	IF @stripeSubId IS NOT NULL
	BEGIN
		-- If one exists, delete the subscription plan
		Update [Billing].[StripeCustomerSubscriptionPlan]
		SET [IsActive] = 0 
		WHERE [StripeTokenSubId] = @stripeSubId; 

		-- ...and add a history item
		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @description, @organizationId, @userId, @skuId) 

		SELECT @stripeSubId -- On success, return subscription plan id
	END
END
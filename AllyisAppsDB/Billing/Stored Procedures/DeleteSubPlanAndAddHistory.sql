CREATE PROCEDURE [Billing].[DeleteSubPlanAndAddHistory]
	@CustomerId NVARCHAR(50), 
	@OrganizationId INT,
	@UserId INT,
	@SkuId INT,
	@Description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the stripe token subscription id
	DECLARE @StripeSubId NCHAR(50);
	SELECT @StripeSubId = [StripeCustomerSubscriptionPlan].[StripeTokenSubId]
	FROM [Billing].[StripeCustomerSubscriptionPlan] WITH (NOLOCK) 
	WHERE [StripeTokenCustId] = @CustomerId AND [OrganizationId] = @OrganizationId AND [IsActive] = 1;

	IF @StripeSubId IS NOT NULL
	BEGIN
		-- If one exists, delete the subscription plan
		Update [Billing].[StripeCustomerSubscriptionPlan]
		SET [IsActive] = 0 
		WHERE [StripeTokenSubId] = @StripeSubId; 

		-- ...and add a history item
		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @Description, @OrganizationId, @UserId, @SkuId) 

		SELECT @StripeSubId -- On success, return subscription plan id
	END
END
CREATE PROCEDURE [Billing].[UpdateCustomerSubscription]
	@customerID NVARCHAR(50),
	@SubPlanId NVARCHAR(50),
	@NumberOfUsers INT,
	@Price INT,
	@OrganizationId INT,
	@UserId INT,
	@SkuId INT,
	@Description NVARCHAR(MAX)
AS
	SET NOCOUNT ON;
BEGIN
	BEGIN TRANSACTION
		UPDATE [Billing].[StripeCustomerSubscriptionPlan] 
		SET [StripeCustomerSubscriptionPlan].[NumberOfUsers] = @NumberOfUsers,
		[StripeCustomerSubscriptionPlan].[Price] = @Price
		WHERE [StripeTokenCustId] = @customerID
		AND [StripeTokenSubId] = @SubPlanId;

		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @Description, @OrganizationId, @UserId, @SkuId)
	COMMIT
END
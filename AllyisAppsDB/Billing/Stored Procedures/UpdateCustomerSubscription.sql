CREATE PROCEDURE [Billing].[UpdateCustomerSubscription]
	@customerId NVARCHAR(50),
	@subPlanId NVARCHAR(50),
	@numberOfUsers INT,
	@price INT,
	@organizationId INT,
	@userId INT,
	@skuId INT,
	@description NVARCHAR(MAX)
AS
	SET NOCOUNT ON;
BEGIN
	BEGIN TRANSACTION
		UPDATE [Billing].[StripeCustomerSubscriptionPlan] 
		SET [StripeCustomerSubscriptionPlan].[NumberOfUsers] = @numberOfUsers,
		[StripeCustomerSubscriptionPlan].[Price] = @price
		WHERE [StripeTokenCustId] = @customerId
		AND [StripeTokenSubId] = @subPlanId;

		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @description, @organizationId, @userId, @skuId)
	COMMIT
END
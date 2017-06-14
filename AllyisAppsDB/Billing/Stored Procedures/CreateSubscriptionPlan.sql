CREATE PROCEDURE [Billing].[CreateSubscriptionPlan]
	@OrganizationId INT,
	@StripeTokenCustId NVARCHAR(50),
	@StripeTokenSubId NVARCHAR(50), 
	@NumberOfUsers INT, 
	@Price INT,
	@ProductId INT,
	@UserId INT,
	@SkuId INT,
	@Description NVARCHAR(MAX)
AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		INSERT INTO [Billing].[StripeCustomerSubscriptionPlan] (
			[OrganizationId],
			[StripeTokenCustId],
			[StripeTokenSubId],
			[NumberOfUsers],
			[Price],
			[ProductId],
			[IsActive])
		VALUES (
			@OrganizationId,
			@StripeTokenCustId,
			@StripeTokenSubId,
			@NumberOfUsers,
			@Price,
			@ProductId,
			1);

		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @Description, @OrganizationId, @UserId, @SkuId)
	COMMIT
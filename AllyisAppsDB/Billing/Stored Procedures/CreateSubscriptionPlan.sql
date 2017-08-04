CREATE PROCEDURE [Billing].[CreateSubscriptionPlan]
	@organizationId INT,
	@stripeTokenCustId NVARCHAR(50),
	@stripeTokenSubId NVARCHAR(50),
	@numberOfUsers INT,
	@price INT,
	@productId INT,
	@userId INT,
	@skuId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
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
			@organizationId,
			@stripeTokenCustId,
			@stripeTokenSubId,
			@numberOfUsers,
			@price,
			@productId,
			1);

		INSERT INTO [Billing].[BillingHistory]
			([Date],
			[Description],
			[OrganizationId],
			[UserId],
			[SkuId])
		VALUES
			(SYSDATETIME(),
			@description,
			@organizationId,
			@userId,
			@skuId)
	COMMIT
END

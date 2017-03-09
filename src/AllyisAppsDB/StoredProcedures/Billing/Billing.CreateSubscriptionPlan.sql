CREATE PROCEDURE [Billing].[CreateSubscriptionPlan]
	@OrganizationId INT,
	@StripeTokenCustId NVARCHAR(50),
	@StripeTokenSubId NVARCHAR(50), 
	@NumberOfUsers INT, 
	@Price INT,
	@ProductId INT
AS
	
	SET NOCOUNT ON;
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
/*SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];*/
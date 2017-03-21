CREATE PROCEDURE [Billing].[CreateStripeOrgCustomer]
	@OrgId INT,
	@UserId INT,
	@CustomerId NVARCHAR(50),
	@SkuId INT,
	@Description NVARCHAR(MAX)
AS
BEGIN TRANSACTION
	INSERT INTO [Billing].[StripeOrganizationCustomer] ([OrganizationId], [StripeTokenCustId], [IsActive])
	VALUES (@OrgId, @CustomerId, 1)

	INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
	VALUES (SYSDATETIME(), @Description, @OrgId, @UserId, @SkuId)
COMMIT
CREATE PROCEDURE [Billing].[CreateStripeOrganizationCustomer]
	@organizationId INT,
	@userId INT,
	@customerId NVARCHAR(50),
	@skuId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRANSACTION
		INSERT INTO [Billing].[StripeOrganizationCustomer] ([OrganizationId], [StripeTokenCustId], [IsActive])
		VALUES (@organizationId, @customerId, 1)

		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @description, @organizationId, @userId, @skuId)
	COMMIT TRANSACTION
END

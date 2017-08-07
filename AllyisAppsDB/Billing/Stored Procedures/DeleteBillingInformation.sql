CREATE PROCEDURE [Billing].[DeleteBillingInformation]
	@orgId INT
AS
	SET NOCOUNT ON;
	UPDATE [Billing].[StripeOrganizationCustomer]
	SET [IsActive] = 0
	WHERE [OrganizationId] = @orgId
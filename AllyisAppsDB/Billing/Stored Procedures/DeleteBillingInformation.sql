CREATE PROCEDURE [Billing].[DeleteBillingInformation]
	@OrgId INT
AS
	SET NOCOUNT ON;
	UPDATE [Billing].[StripeOrganizationCustomer]
	SET [IsActive] = 0
	WHERE [OrganizationId] = @OrgId
CREATE PROCEDURE [Billing].[GetStripeOrgCustomer]
	@OrgId INT
AS
	SET NOCOUNT ON;
	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrgId AND [IsActive] = 1
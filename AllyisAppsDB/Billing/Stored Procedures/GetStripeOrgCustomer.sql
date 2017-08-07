CREATE PROCEDURE [Billing].[GetStripeOrgCustomer]
	@orgId INT
AS
	SET NOCOUNT ON;
	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1
CREATE PROCEDURE [Billing].[CreateStripeOrgCustomer]
	@OrgId INT, 
	@CustomerId NVARCHAR(50)
AS
	INSERT INTO [Billing].[StripeOrganizationCustomer] ([OrganizationId], [StripeTokenCustId], [IsActive])
	VALUES (@OrgId, @CustomerId, 1)
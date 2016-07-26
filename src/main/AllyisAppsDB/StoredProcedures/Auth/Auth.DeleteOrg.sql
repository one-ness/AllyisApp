CREATE PROCEDURE [Auth].[DeleteOrg]
	@OrgId INT
AS 
BEGIN
BEGIN TRANSACTION

		UPDATE [Auth].[Organization]
		SET [IsActive] = 0, [Subdomain] = NULL
		WHERE [OrganizationId] = @OrgId;

		UPDATE [Billing].[Subscription] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @OrgId;

		UPDATE [Crm].[Customer] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @OrgId;

		UPDATE [Billing].[StripeCustomerSubscriptionPlan] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @OrgId;

		UPDATE [Crm].[Project] 
		SET [IsActive] = 0
		WHERE (SELECT [OrganizationId]
			FROM [Crm].[Customer] WITH (NOLOCK) 
			WHERE [Customer].[CustomerId] = [Project].[CustomerId])
		= @OrgId;

		UPDATE [Crm].[ProjectUser] 
		SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] IN 
			(SELECT [ProjectId] 
			FROM [Crm].[Project] WITH (NOLOCK) 
			WHERE [IsActive] = 0);

		UPDATE [Billing].[StripeOrganizationCustomer] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @OrgId;

		UPDATE [Auth].[Invitation] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @OrgId;

	COMMIT TRANSACTION	
END
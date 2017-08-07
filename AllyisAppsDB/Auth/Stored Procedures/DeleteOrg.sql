CREATE PROCEDURE [Auth].[DeleteOrg]
	@orgId INT
AS 
BEGIN
BEGIN TRANSACTION

		UPDATE [Auth].[Organization]
		SET [IsActive] = 0, [Subdomain] = NULL
		WHERE [OrganizationId] = @orgId;

		UPDATE [Billing].[Subscription] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;

		UPDATE [Crm].[Customer] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;

		UPDATE [Billing].[StripeCustomerSubscriptionPlan] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;

		UPDATE [Pjm].[Project] 
		SET [IsActive] = 0
		WHERE (SELECT [OrganizationId]
			FROM [Crm].[Customer] WITH (NOLOCK) 
			WHERE [Customer].[CustomerId] = [Project].[CustomerId])
		= @orgId;

		UPDATE [Pjm].[ProjectUser] 
		SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] IN 
			(SELECT [ProjectId] 
			FROM [Pjm].[Project] WITH (NOLOCK) 
			WHERE [IsActive] = 0);

		UPDATE [Billing].[StripeOrganizationCustomer] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;

		UPDATE [Auth].[Invitation] 
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;

	COMMIT TRANSACTION	
END
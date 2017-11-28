CREATE PROCEDURE [Auth].[DeleteOrganizationUsers]
	@organizationId	INT,
	@userIds [Auth].[UserTable] READONLY
AS
	
	DELETE FROM [Pjm].[ProjectUser] WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [ProjectId] IN 
		(SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN
			(SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = @organizationId));

	DELETE FROM [Billing].[SubscriptionUser] WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [SubscriptionId] IN 
		(SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] = @organizationId);
	
	DELETE FROM [Auth].[OrganizationUser] WHERE [OrganizationUser].[UserId] IN (SELECT [userId] FROM @userIds) AND [OrganizationUser].[OrganizationId] = @organizationId;

	SELECT @@ROWCOUNT;
	

CREATE PROCEDURE [Auth].[DeleteOrgUser]
	@organizationId INT,
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Pjm].[ProjectUser] WHERE [UserId] = @userId AND [ProjectId] IN 
		(SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN
			(SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = @organizationId));

	DELETE FROM [Billing].[SubscriptionUser] WHERE [UserId] = @userId AND [SubscriptionId] IN 
		(SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] = @organizationId);
	
	DELETE FROM [Auth].[OrganizationUser] WHERE [OrganizationUser].[UserId] = @userId AND [OrganizationUser].[OrganizationId] = @organizationId

	
	--DECLARE @subscriptionId INT = 
	--	(SELECT TOP 1 ([SubscriptionId]) 
	--	FROM [Billing].[Subscription] 
	--	WITH (NOLOCK)
	--	WHERE [OrganizationId] = @organizationId 
	--	ORDER BY [SubscriptionId] DESC);
	--DELETE FROM [Billing].[SubscriptionUser] WHERE [SubscriptionId] = @subscriptionId AND [UserId] = @userId
END
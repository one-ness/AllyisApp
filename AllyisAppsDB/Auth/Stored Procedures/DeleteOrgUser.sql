CREATE PROCEDURE [Auth].[DeleteOrgUser]
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Pjm].[ProjectUser] WHERE [UserId] = @UserId AND [ProjectId] IN 
		(SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN
			(SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = @OrganizationId));

	DELETE FROM [Billing].[SubscriptionUser] WHERE [UserId] = @UserId AND [SubscriptionId] IN 
		(SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] = @OrganizationId);
	
	DELETE FROM [Auth].[OrganizationUser] WHERE [OrganizationUser].[UserId] = @UserId AND [OrganizationUser].[OrganizationId] = @OrganizationId

	
	--DECLARE @SubscriptionId INT = 
	--	(SELECT TOP 1 ([SubscriptionId]) 
	--	FROM [Billing].[Subscription] 
	--	WITH (NOLOCK)
	--	WHERE [OrganizationId] = @OrganizationId 
	--	ORDER BY [SubscriptionId] DESC);
	--DELETE FROM [Billing].[SubscriptionUser] WHERE [SubscriptionId] = @SubscriptionId AND [UserId] = @UserId
END
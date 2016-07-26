CREATE PROCEDURE [Auth].[GetUsersWithSubscriptionToProductInOrganization]
	@OrganizationId INT,
	@ProductId INT 
AS
	SET NOCOUNT ON;
SELECT [User].[UserId],
	   [User].[FirstName],
	   [User].[LastName],
	   [User].[Email]
FROM [Auth].[User] WITH (NOLOCK) 
LEFT JOIN [Billing].[SubscriptionUser]	WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
WHERE 
	[Subscription].[SubscriptionId] = (
	SELECT [SubscriptionId] 
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]		WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId
		AND [Sku].[ProductId] = @ProductId
		AND [Subscription].[IsActive] = 1
	)

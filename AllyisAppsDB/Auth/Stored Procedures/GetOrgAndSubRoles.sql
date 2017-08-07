CREATE PROCEDURE [Auth].[GetOrgAndSubRoles]
	@organizationId INT
AS
	SELECT
		[User].[FirstName],
		[User].[LastName],
		[User].[UserId],
		[OrganizationUser].[OrganizationRoleId],
		[OrganizationRole].[OrganizationRoleName],
		[User].[Email],
		[SubscriptionUser].[ProductRoleId], 
		[SubscriptionUser].[SubscriptionId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[UserId] = [User].[UserId]
	INNER JOIN [Auth].[OrganizationRole]				WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[OrganizationId] = @organizationId
	LEFT JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) 
											ON [SubscriptionUser].[UserId] = [User].[UserId]
											AND [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
	WHERE [OrganizationUser].[OrganizationId] = @organizationId
	ORDER BY [User].[LastName]

	SELECT 
		[Subscription].[SubscriptionId],
		[Sku].[ProductId],
		[Product].[ProductName] AS 'ProductName'
	FROM [Billing].[Subscription] WITH (NOLOCK)
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [OrganizationId] = @organizationId AND [Subscription].[IsActive] = 1
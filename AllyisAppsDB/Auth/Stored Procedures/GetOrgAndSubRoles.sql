CREATE PROCEDURE [Auth].[GetOrgAndSubRoles]
	@OrganizationId INT
AS
	SELECT
		[User].[FirstName],
		[User].[LastName],
		[User].[UserId],
		[OrganizationUser].[OrgRoleId],
		[OrgRole].[Name],
		[User].[Email],
		[SubscriptionUser].[ProductRoleId], 
		[SubscriptionUser].[SubscriptionId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[UserId] = [User].[UserId]
	INNER JOIN [Auth].[OrgRole]				WITH (NOLOCK) ON [OrgRole].[OrgRoleId] = [OrganizationUser].[OrgRoleId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[OrganizationId] = @OrganizationId
	LEFT JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) 
											ON [SubscriptionUser].[UserId] = [User].[UserId]
											AND [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
	ORDER BY [User].[LastName]

	SELECT 
		[Subscription].[SubscriptionId],
		[Sku].[ProductId],
		[Product].[Name] AS 'ProductName'
	FROM [Billing].[Subscription] WITH (NOLOCK)
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1
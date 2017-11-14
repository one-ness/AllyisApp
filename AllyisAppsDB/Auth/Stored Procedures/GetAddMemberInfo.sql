CREATE PROCEDURE [Auth].[GetAddMemberInfo]
	@organizationId INT
AS
	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @organizationId
	ORDER BY [EmployeeId] DESC

	SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @organizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[ProductRole].[ProductRoleName],
		[ProductRole].[ProductRoleId],
		[ProductRole].[ProductId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Subscription].[IsActive] = 1

	

	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @organizationId
	ORDER BY [EmployeeId] DESC
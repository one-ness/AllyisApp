CREATE PROCEDURE [Auth].[GetAddMemberInfo]
	@OrganizationId INT
AS
	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
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
	WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[ProductRole].[ProductRoleName],
		[ProductRole].[ProductRoleId],
		[ProductRole].[ProductId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
	FROM (
		[Auth].[Organization]	WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrganizationId)
		JOIN [Pjm].[Project]	WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[ProjectName]

	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1
	ORDER BY [EmployeeId] DESC
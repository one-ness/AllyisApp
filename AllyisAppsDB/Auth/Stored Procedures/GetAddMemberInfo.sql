CREATE PROCEDURE [Auth].[GetAddMemberInfo]
	@OrganizationId INT
AS
	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
	ORDER BY [EmployeeId] DESC

	SELECT	[Product].[ProductId],
		[Product].[Name] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[Name] AS [OrganizationName],
		[Sku].[Name] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[Name]

	SELECT 
		[ProductRole].[Name],
		[ProductRole].[ProductRoleId],
		[ProductRole].[ProductId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[CreatedUtc],
		[Project].[Name] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[Name] AS [OrganizationName],
		[Customer].[Name] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[Type] AS [PriceType]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrganizationId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[Name]

	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1
	ORDER BY [EmployeeId] DESC
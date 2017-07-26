CREATE PROCEDURE [Auth].[GetOrgManagementInfo]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[OrganizationId],
		[Organization].[Name],
		[SiteUrl], 
		[Address], 
		[City], 
		[State].[Name] AS 'State', 
		[Country].[Name] AS 'Country', 
		[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[CreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Organization].[Country]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Organization].[State]
	WHERE OrganizationId = @OrganizationId

	SELECT [OU].[OrganizationId],
	    [OU].[UserId],
		[OU].[OrganizationRoleId],
		[O].[Name] AS [OrganizationName],
		[OU].[EmployeeId],
		[U].[Email],
		[U].[FirstName],
		[U].[LastName],
		[OU].[EmployeeTypeId]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @OrganizationId
	ORDER BY [U].[LastName]

	SELECT	[Product].[ProductId],
		[Product].[Name] AS [ProductName],
		[Product].[AreaUrl],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[Name] AS [OrganizationName],
		[Sku].[Name] AS [SkuName],
		(SELECT COUNT([UserId])
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
			WHERE [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId])
			AS SubscriptionsUsed
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[Name]

	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[Invitation].[OrganizationRoleId],
		[Name] AS [OrganizationRoleName],
		[EmployeeId],
		[EmployeeTypeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1

	SELECT
		[Product].[ProductId],
		[Sku].[Name],
		[Product].[Description],
		[Product].[AreaUrl]
	FROM [Billing].[Product] WITH (NOLOCK) 
	INNER JOIN [Billing].[Sku] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	RIGHT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	WHERE [Product].[IsActive] = 1 AND [Subscription].[IsActive] = 1 AND [Subscription].OrganizationId = @OrganizationId
	ORDER BY [Product].[Name]
END
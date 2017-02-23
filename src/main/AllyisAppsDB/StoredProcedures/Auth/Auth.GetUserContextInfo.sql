CREATE PROCEDURE [Auth].[GetUserContextInfo]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [U].[UserId],
		   [U].[FirstName],
		   [U].[LastName],
		   [U].[UserName],
		   [U].[Email],
		   [U].[LastSubscriptionId],
		   [U].[ActiveOrganizationId],
		   [U].[LanguagePreference],
		   [O].[OrganizationId],
		   [O].[Name] AS 'OrganizationName',
		   [OU].[OrgRoleId],
		   [SUB].[SubscriptionId],
		   [SUB].[ProductId],
		   [SUB].[ProductName],
		   [SUB].[SkuId],
		   [SUB].[ProductRoleId]
	FROM [Auth].[User] AS [U] WITH (NOLOCK)
		LEFT JOIN [Auth].[OrganizationUser] AS [OU] WITH (NOLOCK) ON [U].[UserId] = [OU].[UserId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [OU].[OrganizationId] = [O].[OrganizationId]
		LEFT JOIN (
			SELECT	[S].[SubscriptionId],
					[PR].[ProductId],
					[P].[Name] AS 'ProductName',
					[PR].[Name] AS 'RoleName',
					[S].[SkuId],
					[SU].[ProductRoleId],
					[SU].[UserId],
					[S].[OrganizationId]
			FROM [Billing].[SubscriptionUser] AS [SU] WITH (NOLOCK)
				JOIN [Billing].[Subscription] AS [S] WITH (NOLOCK) ON [SU].[SubscriptionId] = [S].SubscriptionId
				JOIN [Auth].[ProductRole] AS [PR] WITH (NOLOCK) ON [SU].[ProductRoleId] = [PR].[ProductRoleId]
				LEFT JOIN [Billing].[Product] AS [P] WITH (NOLOCK) ON [PR].[ProductId] = [P].[ProductId]
		) [SUB] ON [SUB].[UserId] = [U].[UserId] AND [SUB].[OrganizationId] = [O].[OrganizationId]
	WHERE [U].[UserId] = @UserId;
END
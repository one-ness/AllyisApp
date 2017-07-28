﻿CREATE PROCEDURE [Auth].[GetUserContextInfo]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [U].[UserId],
		   [U].[FirstName],
		   [U].[LastName],
		   [U].[Email],
		   [U].[LastSubscriptionId],
		   [U].[ActiveOrganizationId],
		   [U].[LanguagePreference],
		   [O].[OrganizationId],
		   [O].[Name] AS 'OrganizationName',
		   [OU].[OrganizationRoleId],
		   [SUB].[SubscriptionId],
		   [SUB].[SubscriptionName],
		   [SUB].[ProductId],
		   [SUB].[ProductName],
		   [SUB].[SkuId],
		   [SUB].[ProductRoleId],
		   [SUB].[AreaUrl]
	FROM [Auth].[User] AS [U] WITH (NOLOCK)
		LEFT JOIN [Auth].[OrganizationUser] AS [OU] WITH (NOLOCK) ON [U].[UserId] = [OU].[UserId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [OU].[OrganizationId] = [O].[OrganizationId]
		LEFT JOIN (
			SELECT	[S].[SubscriptionId],
					[S].[SubscriptionName],
					[PR].[ProductId],
					[P].[Name] AS 'ProductName',
					[P].[AreaUrl],
					[PR].[Name] AS 'ProductRoleName',
					[S].[SkuId],
					[SU].[ProductRoleId],
					[SU].[UserId],
					[S].[OrganizationId]
			FROM [Billing].[SubscriptionUser] AS [SU] WITH (NOLOCK)
				JOIN [Billing].[Subscription] AS [S] WITH (NOLOCK) ON [SU].[SubscriptionId] = [S].SubscriptionId
				JOIN [Auth].[ProductRole] AS [PR] WITH (NOLOCK) ON [SU].[ProductRoleId] = [PR].[ProductRoleId]
				LEFT JOIN [Billing].[Product] AS [P] WITH (NOLOCK) ON [PR].[ProductId] = [P].[ProductId]
			WHERE [S].[IsActive] = 1
		) [SUB] ON [SUB].[UserId] = [U].[UserId] AND [SUB].[OrganizationId] = [O].[OrganizationId]
	WHERE [U].[UserId] = @UserId;
END
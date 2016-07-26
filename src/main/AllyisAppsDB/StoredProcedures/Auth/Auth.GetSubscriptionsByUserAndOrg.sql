CREATE PROCEDURE [Auth].[GetSubscriptionsByUserAndOrg]
	@UserId INT,
	@OrgId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[User].[UserId],
	[Subscription].[SubscriptionId],
	[Subscription].[OrganizationId],
	[Subscription].[SkuId],
	[ProductRole].[ProductRoleId],
	[ProductRole].[Name] AS [ProductRoleName],
	[ProductRole].[PermissionAdmin]
	FROM
	(
		(SELECT [Auth].[User].[UserId] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @UserId) AS [User]
		JOIN [Billing].[SubscriptionUser]	WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
		JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		JOIN [Billing].[Sku]				WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
		JOIN [Auth].[ProductRole]			WITH (NOLOCK) ON [ProductRole].[ProductRoleId] = [SubscriptionUser].[ProductRoleId]
		JOIN [Auth].[OrganizationUser]			WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [OrganizationUser].[UserId]
	)
	WHERE [Subscription].[IsActive] = 1 AND [OrganizationUser].[OrganizationId] = @OrgId
END


CREATE PROCEDURE [Auth].[GetRolesAndPermissions]
	@OrgId INT
AS 
BEGIN
	SET NOCOUNT ON;

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
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[OrganizationId] = @OrgId
	LEFT JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) 
											ON [SubscriptionUser].[UserId] = [User].[UserId]
											AND [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
	WHERE [OrganizationUser].[OrganizationId] = @OrgId
	ORDER BY [User].[LastName]
END
CREATE PROCEDURE [Auth].[GetUsersByOrganization]
	@OrganizationId INT,
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
LEFT JOIN (SELECT [UserId], [ProductRoleId] 
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
			WHERE [SubscriptionId] = @SubscriptionId)
			AS [OnRoles]
			ON [OnRoles].[UserId] = [User].[UserId]
WHERE [OrganizationId] = @OrganizationId 
CREATE PROCEDURE [Crm].[GetNextProjectIdAndSubUsers]
	@CustomerId INT,
	@SubscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[ProjectOrgId]
	FROM [Crm].[Project] WITH (NOLOCK)
	WHERE [Project].[CustomerId] = @CustomerId
	ORDER BY [ProjectOrgId] DESC

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @SubscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = (
		SELECT TOP 1
			[OrganizationId]
		FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerId] = @CustomerId
	) AND [ProductRoleId] IS NOT NULL
	ORDER BY [User].[LastName]
END
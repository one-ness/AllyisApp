CREATE PROCEDURE [Pjm].[GetNextProjectIdAndSubUsers]
	@customerId INT,
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[ProjectOrgId]
	FROM [Pjm].[Project] WITH (NOLOCK)
	WHERE [Project].[CustomerId] = @customerId
	ORDER BY [ProjectOrgId] DESC

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @subscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = (
		SELECT TOP 1
			[OrganizationId]
		FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerId] = @customerId
	) AND [ProductRoleId] IS NOT NULL
	ORDER BY [User].[LastName]
END
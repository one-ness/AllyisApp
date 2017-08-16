CREATE PROCEDURE [TimeTracker].[GetReportInfo]
	@orgId INT,
	@subscriptionId INT
AS
	SET NOCOUNT ON
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'State',
		   [State].[StateId],
		   [Country].[CountryName] AS 'Country',
		   [Country].[CountryCode],
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @orgId
	ORDER BY [Customer].[CustomerName]

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
	FROM [Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @orgId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @subscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = @orgId AND [ProductRoleId] IS NOT NULL
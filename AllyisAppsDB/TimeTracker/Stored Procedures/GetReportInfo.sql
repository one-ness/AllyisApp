CREATE PROCEDURE [TimeTracker].[GetReportInfo]
	@OrgId INT,
	@SubscriptionId INT
AS
	SET NOCOUNT ON
	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Customer].[Address],
		   [Customer].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [Customer].[PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Customer].[Country]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Customer].[State]
	WHERE [Customer].[OrganizationId] = @OrgId
	ORDER BY [Customer].[Name]

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
	FROM [Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Crm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @SubscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = @OrgId AND [ProductRoleId] IS NOT NULL
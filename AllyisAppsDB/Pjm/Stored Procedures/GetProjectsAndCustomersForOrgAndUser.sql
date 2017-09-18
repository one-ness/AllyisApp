CREATE PROCEDURE [Pjm].[GetProjectsAndCustomersForOrgAndUser]
	@orgId int,
	@userId int
AS
	SET NOCOUNT ON

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
		[Customer].[CustomerId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly],
		[SUB].[IsProjectUser]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @orgId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
		LEFT JOIN (
			SELECT 1 AS 'IsProjectUser',
			[ProjectUser].[ProjectId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[UserId] = @userId
		) [SUB] ON [SUB].[ProjectId] = [Project].[ProjectId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[ProjectName]

	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1],
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId],
		   [Customer].[OrganizationId],
		   [Customer].[IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @orgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[CustomerName]
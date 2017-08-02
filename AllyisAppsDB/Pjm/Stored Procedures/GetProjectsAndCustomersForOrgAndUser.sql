CREATE PROCEDURE [Pjm].[GetProjectsAndCustomersForOrgAndUser]
	@OrgId int,
	@UserId int
AS
	SET NOCOUNT ON

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
		[Project].[IsHourly] AS [IsHourly],
		[SUB].[IsProjectUser]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
		LEFT JOIN (
			SELECT 1 AS 'IsProjectUser',
			[ProjectUser].[ProjectId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[UserId] = @UserId
		) [SUB] ON [SUB].[ProjectId] = [Project].[ProjectId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[Name]

	SELECT [Customer].[CustomerId],
		   [Customer].[Name],
		   [Address1],
		   [City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[Name]
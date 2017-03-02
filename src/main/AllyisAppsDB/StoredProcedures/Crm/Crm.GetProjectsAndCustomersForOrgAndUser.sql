CREATE PROCEDURE [Crm].[GetProjectsAndCustomersForOrgAndUser]
	@OrgId int,
	@UserId int
AS
	SET NOCOUNT ON

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[CreatedUTC],
		[Project].[Name] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[Name] AS [OrganizationName],
		[Customer].[Name] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[Type] AS [PriceType],
		[SUB].[IsProjectUser]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Crm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
		LEFT JOIN (
			SELECT 1 AS 'IsProjectUser',
			[ProjectUser].[ProjectId]
			FROM [Crm].[ProjectUser]
			WHERE [ProjectUser].[UserId] = @UserId
		) [SUB] ON [SUB].[ProjectId] = [Project].[ProjectId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[Name]

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
		   [Customer].[CreatedUTC],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Customer].[Country]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Customer].[State]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[Name]
CREATE PROCEDURE [Pjm].[GetProjectByProjectOrgId]
	@projectOrgId NVARCHAR
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[IsHourly] AS [PriceType],
			[Project].[IsActive],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectOrgId] = @projectOrgId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)
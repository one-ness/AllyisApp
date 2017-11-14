CREATE PROCEDURE [Pjm].[GetProjectById]
	@projectId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerCode],
			[Project].[IsHourly] AS [PriceType],
			[Project].[IsActive],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectCode]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectCode] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @projectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)
CREATE PROCEDURE [Pjm].[GetProjectById]
	@projectId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Project].[IsDefault],
			[Project].[IsHourly] AS [PriceType],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectCode],
			[Project].[ProjectName],
			[Project].[ProjectCreatedUtc],
			[Customer].[OrganizationId],
			[Customer].[CustomerName],
			[Customer].[CustomerCode],
			[Organization].[OrganizationName]
	FROM [Pjm].[Project] WITH (NOLOCK)
	JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	WHERE [ProjectId] = @projectId
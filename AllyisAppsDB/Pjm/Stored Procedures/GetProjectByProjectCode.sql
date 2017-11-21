CREATE PROCEDURE [Pjm].[GetProjectByProjectCode]
	@projectCode NVARCHAR
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Project].[IsHourly] AS [PriceType],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectCode],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName],
			[Customer].[OrganizationId],
			[Customer].[CustomerName],
			[Customer].[CustomerCode],
			[Organization].[OrganizationName]
	FROM [Pjm].[Project] WITH (NOLOCK)
	JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	WHERE [ProjectCode] = @projectCode
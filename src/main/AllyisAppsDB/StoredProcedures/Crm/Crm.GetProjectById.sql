CREATE PROCEDURE [Crm].[GetProjectById]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[CreatedUTC],
			[Project].[Name] AS [ProjectName],
			[Organization].[Name] AS [OrganizationName],
			[Customer].[Name] AS [CustomerName],
			[Project].[Type] AS [PriceType],
			[Project].[StartUTC] AS [StartDate],
			[Project].[EndUTC] AS [EndDate],
			[Project].[ProjectId]
			FROM (
		(SELECT [ProjectId], [CustomerId], [Name], [Type], [StartUTC], [EndUTC], [IsActive], 
				[CreatedUTC], [ModifiedUTC] FROM [Crm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @ProjectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)
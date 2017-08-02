CREATE PROCEDURE [Crm].[GetProjectByIdAndUser]
	@ProjectId int,
	@UserId int
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[CreatedUtc],
			[Project].[Name] AS [ProjectName],
			[Organization].[Name] AS [OrganizationName],
			[Customer].[Name] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[IsHourly] AS [PriceType],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId],
			[SUB].[IsProjectUser]
			FROM (
		(SELECT [ProjectId], [CustomerId], [Name], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[CreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @ProjectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
			LEFT JOIN (
				SELECT 1 AS 'IsProjectUser',
				[ProjectUser].[ProjectId]
				FROM [Pjm].[ProjectUser] WITH (NOLOCK)
				WHERE [ProjectUser].[ProjectId] = @ProjectId AND [ProjectUser].[UserId] = @UserId
			) [SUB] ON [SUB].[ProjectId] = @ProjectId
		)
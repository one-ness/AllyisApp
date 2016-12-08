CREATE PROCEDURE [Crm].[GetProjectsByUserId]
	@UserId INT
AS
BEGIN 
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	SELECT [ProjectUser].[ProjectId],
		   [Project].[Name] AS [ProjectName],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerId],
		   [Customer].[Name] AS [CustomerName],
		   [Project].[ProjectOrgId],
		   [Project].[Type] AS [PriceType]
	FROM [Crm].[ProjectUser]  WITH (NOLOCK) 
	LEFT JOIN [Crm].[Project]	WITH (NOLOCK) ON [Project].[ProjectId] = [ProjectUser].[ProjectId]
	LEFT JOIN [Crm].[Customer]	WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	WHERE [Customer].[IsActive] = 1 
		AND [Project].[IsActive] = 1
		AND [ProjectUser].[IsActive] = 1
		AND [ProjectUser].[UserId] = @UserId
	ORDER BY [Project].[Name]
	COMMIT TRANSACTION
END
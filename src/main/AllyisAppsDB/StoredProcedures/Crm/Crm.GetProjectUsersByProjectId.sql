CREATE PROCEDURE [Crm].[GetProjectUsersByProjectId]
	@ProjectId INT
AS
BEGIN 
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	SELECT [ProjectUser].[UserId], [FirstName], [LastName]
	FROM [Crm].[ProjectUser] WITH (NOLOCK) 
	LEFT JOIN [Crm].[Project]	WITH (NOLOCK) ON [Project].[ProjectId] = [ProjectUser].[ProjectId]
	LEFT JOIN [Crm].[Customer]	WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [ProjectUser].[UserId]

	WHERE [Customer].[IsActive] = 1 
		AND [Project].[IsActive] = 1
		AND [ProjectUser].[IsActive] = 1
		AND [ProjectUser].[ProjectId] = @ProjectId
	COMMIT TRANSACTION
END
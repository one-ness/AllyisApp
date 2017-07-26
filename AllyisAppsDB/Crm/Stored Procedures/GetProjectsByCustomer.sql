CREATE PROCEDURE [Crm].[GetProjectsByCustomer]
	@CustomerId INT
AS
	SET NOCOUNT ON;
	SELECT [Name],
		   [ProjectId],
		   [ProjectOrgId],
		   [Type],
		   [CustomerId],
		   [StartUtc] AS [StartingDate],
		   [EndUtc] AS [EndingDate]
	FROM [Crm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 1 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[Name]
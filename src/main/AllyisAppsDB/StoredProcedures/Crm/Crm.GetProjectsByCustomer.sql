CREATE PROCEDURE [Crm].[GetProjectsByCustomer]
	@CustomerId INT
AS
	SET NOCOUNT ON;
	SELECT [Name],
		   [ProjectId],
		   [ProjectOrgId]
	FROM [Crm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 1 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[Name]
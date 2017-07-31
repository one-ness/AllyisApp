CREATE PROCEDURE [Pjm].[GetInactiveProjectsByCustomer]
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
	FROM [Pjm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 0 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[Name]

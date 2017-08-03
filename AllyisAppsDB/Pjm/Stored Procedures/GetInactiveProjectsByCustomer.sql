CREATE PROCEDURE [Pjm].[GetInactiveProjectsByCustomer]
	@CustomerId INT
AS
	SET NOCOUNT ON;
	SELECT [ProjectName],
		   [ProjectId],
		   [ProjectOrgId],
		   [IsHourly],
		   [CustomerId],
		   [StartUtc] AS [StartingDate],
		   [EndUtc] AS [EndingDate]
	FROM [Pjm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 0 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[ProjectName]

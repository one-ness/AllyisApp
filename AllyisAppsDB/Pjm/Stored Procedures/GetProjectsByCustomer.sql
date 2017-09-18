CREATE PROCEDURE [Pjm].[GetProjectsByCustomer]
	@customerId INT
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
	WHERE [IsActive] = 1 AND [CustomerId] = @customerId
	ORDER BY [Project].[ProjectName]
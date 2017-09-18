CREATE PROCEDURE [Pjm].[GetInactiveProjectsByCustomer]
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
	WHERE [IsActive] = 0 AND [CustomerId] = @customerId
	ORDER BY [Project].[ProjectName]

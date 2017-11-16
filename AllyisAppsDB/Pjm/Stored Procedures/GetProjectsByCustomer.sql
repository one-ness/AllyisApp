CREATE PROCEDURE [Pjm].[GetProjectsByCustomer]
	@customerId INT
AS
	SET NOCOUNT ON;
	SELECT [ProjectName],
		   [ProjectId],
		   [ProjectCode],
		   [IsHourly],
		   [CustomerId],
		   [StartUtc] AS [StartingDate],
		   [EndUtc] AS [EndingDate]
	FROM [Pjm].[Project] WITH (NOLOCK) 
	WHERE [CustomerId] = @customerId
	ORDER BY [Project].[ProjectName]
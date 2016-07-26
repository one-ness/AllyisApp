CREATE PROCEDURE [TimeTracker].[GetOrganizationFromProject]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	SELECT [Customer].[OrganizationId] FROM [Crm].[Customer] WITH (NOLOCK) INNER JOIN [Crm].[Project] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [ProjectId] = @ProjectId;

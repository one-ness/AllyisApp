CREATE PROCEDURE [Pjm].[GetNextProjectId]
	@orgId INT,
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[ProjectCode]
	FROM [Pjm].[Project] WITH (NOLOCK) Join
	[Crm].[Customer] ON [Crm].[Customer].[CustomerId] = [Pjm].[Project].[CustomerId]
	WHERE [Customer].[OrganizationId] = @orgId
	ORDER BY [ProjectCode] DESC
END
CREATE PROCEDURE [TimeTracker].[GetPayClassByNameAndOrg]
	@Name varchar(50),
	@OrganizationId int
AS
	SET NOCOUNT ON;
	SELECT TOP 1 [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [Name] = @Name AND [OrganizationId] = @OrganizationId;

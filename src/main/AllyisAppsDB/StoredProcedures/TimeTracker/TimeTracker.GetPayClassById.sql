CREATE PROCEDURE [TimeTracker].[GetPayClassById]
	@ID INT = 0
AS
	SET NOCOUNT ON;
	SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [PayClassID] = @ID;

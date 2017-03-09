CREATE PROCEDURE [TimeTracker].[GetPayClasses]
	@OrganizationId INT = 0
AS
	SET NOCOUNT ON;
	IF(SELECT COUNT(*) FROM [TimeTracker].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

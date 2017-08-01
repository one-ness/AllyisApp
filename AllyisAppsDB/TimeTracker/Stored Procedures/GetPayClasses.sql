CREATE PROCEDURE [Hrm].[GetPayClasses]
	@OrganizationId INT = 0
AS
	SET NOCOUNT ON;
	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

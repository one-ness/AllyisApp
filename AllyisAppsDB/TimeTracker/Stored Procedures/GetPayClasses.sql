CREATE PROCEDURE [Hrm].[GetPayClasses]
	@organizationId INT = 0
AS
	SET NOCOUNT ON;
	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @organizationId) > 0
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;
	ELSE
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

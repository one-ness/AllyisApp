CREATE PROCEDURE [Hrm].[GetPayClasses]
	@organizationId INT = 0
AS
	SET NOCOUNT ON;
	SELECT * FROM [Hrm].[PayClass] WITH (NOLOCK)
	WHERE [OrganizationId] = @organizationId;
	

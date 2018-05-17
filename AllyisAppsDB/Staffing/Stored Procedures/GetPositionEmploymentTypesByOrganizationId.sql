CREATE PROCEDURE [Staffing].[GetEmploymentTypesByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [Staffing].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId
	ORDER BY [Staffing].[EmploymentType].[EmploymentTypeName] DESC
END
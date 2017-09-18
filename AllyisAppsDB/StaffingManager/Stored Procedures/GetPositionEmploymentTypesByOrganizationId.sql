CREATE PROCEDURE [StaffingManager].[GetEmploymentTypesByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [StaffingManager].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[EmploymentType].[EmploymentTypeName] DESC
END
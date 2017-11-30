CREATE PROCEDURE [Hrm].[GetEmployeeTypesByOrganization]
	@organizationId INT
AS
BEGIN
	SELECT * FROM [Hrm].[EmployeeType] WHERE [OrganizationId] = @organizationId
END
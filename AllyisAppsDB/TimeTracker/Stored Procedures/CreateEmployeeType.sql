CREATE PROCEDURE [Hrm].[CreateEmployeeType]
	@orgId int,
	@employeeTypeName varchar(64)
AS
BEGIN
	set nocount on
	INSERT INTO [Hrm].[EmployeeType] ([OrganizationId], [EmployeeTypeName]) 
	VALUES (@orgId, @employeeTypeName);

	return SCOPE_IDENTITY(); 
END
CREATE PROCEDURE [Hrm].[CreateEmployeeType]
	@orgId int,
	@employeeTypeName varchar(64)
AS
BEGIN
	set nocount on
	INSERT INTO [Hrm].[EmployeeType] ([OrganizationId], [EmployeeTypeName]) 
	VALUES (@orgId, @employeeTypeName);

	select SCOPE_IDENTITY(); 
END
CREATE PROCEDURE [Hrm].[CreateEmployeeType]
	@orgId int = 0,
	@employeeName varchar(64)
AS
BEGIN
	INSERT INTO [Hrm].[EmployeeType] ([OrganizationId], [EmployeeTypeName]) 
	VALUES (@orgId, @employeeName)
END
BEGIN

	SELECT [EmployeeTypeId] FROM [Hrm].[EmployeeType] WHERE [EmployeeTypeName] = @employeeName
END
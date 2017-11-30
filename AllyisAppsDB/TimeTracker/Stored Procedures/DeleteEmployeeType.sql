CREATE PROCEDURE [Hrm].[DeleteEmployeeType]
	@employeeTypeId INT
AS
	DELETE FROM [Hrm].EmployeeType WHERE [EmployeeTypeId] = @employeeTypeId
RETURN 0

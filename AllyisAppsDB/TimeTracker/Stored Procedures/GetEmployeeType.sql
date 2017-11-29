CREATE PROCEDURE [Hrm].[GetEmployeeType]
@employeeTypeId INT
AS
	SELECT * FROM [Hrm].[EmployeeType] WHERE [EmployeeTypeId] = @employeeTypeId
RETURN 0

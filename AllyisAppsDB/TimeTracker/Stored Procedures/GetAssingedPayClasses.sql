CREATE PROCEDURE [Hrm].[GetAssingedPayClasses]
	@employeeTypeId INT
AS

BEGIN
	SELECT [PayClassId]
	FROM [Hrm].[EmployeeTypePayClass]
	WHERE [EmployeeTypeId] = @employeeTypeId
END


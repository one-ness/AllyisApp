CREATE PROCEDURE [Hrm].[RemovePayClassFromEmployeeType]
	@employeeTypeId int,
	@payClassId int
AS
BEGIN
	DELETE FROM [Hrm].[EmployeeTypePayClass] WHERE [EmployeeTypeId] = @employeeTypeId AND [PayClassId] = @payClassId
END

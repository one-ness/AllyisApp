CREATE PROCEDURE [Hrm].[AddPayClassToEmployeeType]
	@employeeTypeId int,
	@payClassId int
AS
BEGIN
	INSERT INTO [Hrm].[EmployeeTypePayClass] ([EmployeeTypeId], [PayClassId]) VALUES (@employeeTypeId, @payClassId)
END

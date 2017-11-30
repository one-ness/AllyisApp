CREATE PROCEDURE [Hrm].[AddPayClassToEmployeeType]
	@employeeTypeId INT,
	@payClassId INT
AS
BEGIN
	INSERT INTO [Hrm].[EmployeeTypePayClass] ([EmployeeTypeId], [PayClassId]) VALUES (@employeeTypeId, @payClassId)
END

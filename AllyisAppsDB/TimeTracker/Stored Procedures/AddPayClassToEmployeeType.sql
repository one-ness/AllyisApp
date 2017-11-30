CREATE PROCEDURE [Hrm].[AddPayClassToEmployeeType]
	@employeeTypeId int,
	@payClassId int,
	@organizaionId int = 0, 
	@useBuiltIn bit = 0
AS
BEGIN
	if @useBuiltIn = 0
	BEGIN
		INSERT INTO [Hrm].[EmployeeTypePayClass] ([EmployeeTypeId], [PayClassId]) VALUES (@employeeTypeId, @payClassId)
	END
	ELSE
		BEGIN 
			DECLARE @pc as Int
			SELECT @pc = (SELECT PayClass.PayClassId  from PayClass where OrganizationId = @organizaionId AND BuiltInPayClassId = @payClassId);
			INSERT INTO [Hrm].[EmployeeTypePayClass] (EmployeeTypeId, PayClassId)  Values (@employeeTypeId,@pc);
		END
END

CREATE PROCEDURE [Hrm].[CreateDefaultPayClasses]
	@organizationId int
AS
BEGIN
	-- pay class names and builtinpayclassid must match the values used in the application
	SET NOCOUNT ON;
	exec [Hrm].[CreatePayClass] 'Regular', @organizationId, 1
	exec [Hrm].[CreatePayClass] 'Paid Time Off', @organizationId, 2
	exec [Hrm].[CreatePayClass] 'Unpaid Time Off', @organizationId, 3
	exec [Hrm].[CreatePayClass] 'Holiday', @organizationId, 4
	exec [Hrm].[CreatePayClass] 'Overtime', @organizationId, 5
	exec [Hrm].[CreatePayClass] 'Bereavement Leave', @organizationId, 0
	exec [Hrm].[CreatePayClass] 'Jury Duty', @organizationId, 0
	exec [Hrm].[CreatePayClass] 'Other Leave', @organizationId, 0
END
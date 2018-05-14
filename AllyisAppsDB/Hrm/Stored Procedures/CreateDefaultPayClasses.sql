CREATE PROCEDURE [Hrm].[CreateDefaultPayClasses]
	@organizationId int
AS
BEGIN
	-- pay class names and builtinpayclassid must match the values used in the application
	SET NOCOUNT ON;
	exec CreatePayClass 'Regular', @organizationId, 1
	exec CreatePayClass 'Paid Time Off', @organizationId, 2
	exec CreatePayClass 'Unpaid Time Off', @organizationId, 3
	exec CreatePayClass 'Holiday', @organizationId, 4
	exec CreatePayClass 'Overtime', @organizationId, 5
	exec CreatePayClass 'Bereavement Leave', @organizationId, 0
	exec CreatePayClass 'Jury Duty', @organizationId, 0
	exec CreatePayClass 'Other Leave', @organizationId, 0
END
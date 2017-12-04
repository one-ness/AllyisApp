CREATE TABLE [Hrm].[EmployeeTypePayClass]
(
	[EmployeeTypeId] INT NOT NULL,
	[PayClassId] INT NOT NULL,
	CONSTRAINT employee_pay_class UNIQUE ([EmployeeTypeId], [PayClassId])
)

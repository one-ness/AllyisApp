CREATE TABLE [Hrm].[EmployeeTypePayClass] (
	[EmployeeTypeId] INT NOT NULL,
	[PayClassId] INT NOT NULL,
	CONSTRAINT employee_pay_class UNIQUE ([EmployeeTypeId], [PayClassId]), 
    CONSTRAINT [FK_EmployeeTypeID_Employee] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Hrm].[EmployeeType](EmployeeTypeId) ON DELETE CASCADE, 
    CONSTRAINT [FK_EmployeeTypePayClass_PayClass] FOREIGN KEY ([PayClassId]) REFERENCES [Hrm].[PayClass]([PayClassId]) ON DELETE CASCADE
	
	
)

CREATE TABLE [Hrm].[EmployeeType] (
    [EmployeeTypeId] TINYINT       NOT NULL,
    [Name]           NVARCHAR (16) NOT NULL,
    CONSTRAINT [PK_EmployeeType] PRIMARY KEY CLUSTERED ([EmployeeTypeId] ASC)
);


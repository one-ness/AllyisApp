CREATE TABLE [Hrm].[EmployeeType] (
    [EmployeeTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [OrganizationId]   INT           NOT NULL,
    [EmployeeTypeName] NVARCHAR (64) NOT NULL,
    PRIMARY KEY CLUSTERED ([EmployeeTypeId] ASC),
    CONSTRAINT [FK_EmployeeType_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);



GO

CREATE INDEX ORG_INDEX ON [Hrm].[EmployeeType] (OrganizationId);

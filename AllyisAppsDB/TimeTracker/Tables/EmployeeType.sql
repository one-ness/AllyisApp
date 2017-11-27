CREATE TABLE [Hrm].[EmployeeType]
(
	[EmployeeTypeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[OrganizationId] INT NOT NULL,
	[EmployeeTypeName] NVARCHAR(64) NOT NULL,
	CONSTRAINT [FK_ORGANIZATION_ID] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);

GO

CREATE INDEX ORG_INDEX ON [Hrm].[EmployeeType] (OrganizationId);

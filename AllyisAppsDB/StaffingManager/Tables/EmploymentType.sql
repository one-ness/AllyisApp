CREATE TABLE [StaffingManager].[EmploymentType] (
	[EmploymentTypeId]		INT				 IDENTITY (13222, 3) NOT NULL,
	[OrganizationId]		INT				 CONSTRAINT [DF_EmploymentType_Organization] DEFAULT ((0)) NOT NULL,
	[EmploymentTypeName]	NVARCHAR (32)	 NOT NULL,

	CONSTRAINT [PK_EmploymentTypeId] PRIMARY KEY CLUSTERED ([EmploymentTypeId] ASC),
	CONSTRAINT [FK_EmploymentType_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
);


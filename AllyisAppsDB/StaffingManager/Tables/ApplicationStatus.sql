CREATE TABLE [StaffingManager].[ApplicationStatus] (
	[ApplicationStatusId]      INT         NOT NULL IDENTITY (75, 19),
	[OrganizationId]           INT         NOT NULL,
	[ApplicationStatusName] NVARCHAR (128) NOT NULL,
	CONSTRAINT [PK_ApplicationStatus] PRIMARY KEY CLUSTERED ([ApplicationStatusId] ASC),
	CONSTRAINT [FK_ApplicationStatus_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);

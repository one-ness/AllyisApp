CREATE TABLE [StaffingManager].[StaffingSettings] (
	[StaffingSettingsId]			INT				 IDENTITY (13332, 3) NOT NULL,
	[OrganizationId]				INT				 DEFAULT ((0)) NOT NULL,
	[DefaultPositionStatusId]		INT				 NULL,
	[DefaultApplicationStatusId]	INT			 NULL

	CONSTRAINT [PK_StaffingSettingsId] PRIMARY KEY CLUSTERED ([StaffingSettingsId] ASC),
	CONSTRAINT [FK_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
	CONSTRAINT [FK_DefaultStatus] FOREIGN KEY ([DefaultPositionStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId]),
	CONSTRAINT [FK_DefaultApplicationStatus] FOREIGN KEY ([DefaultApplicationStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId])
);


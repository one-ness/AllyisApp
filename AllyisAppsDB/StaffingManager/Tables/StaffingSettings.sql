CREATE TABLE [StaffingManager].[StaffingSettings] (
	[StaffingSettingsId]			INT				 IDENTITY (13332, 3) NOT NULL,
	[SubscriptionId]				INT				 DEFAULT ((0)) NOT NULL,
	[DefaultPositionStatusId]		INT				 NULL,
	[DefaultApplicationStatusId]	INT				 NULL

	CONSTRAINT [PK_StaffingSettingsId] PRIMARY KEY CLUSTERED ([StaffingSettingsId] ASC),
	CONSTRAINT [FK_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]),
	CONSTRAINT [FK_DefaultStatus] FOREIGN KEY ([DefaultPositionStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId]),
	CONSTRAINT [FK_DefaultApplicationStatus] FOREIGN KEY ([DefaultApplicationStatusId]) REFERENCES [StaffingManager].[ApplicationStatus] ([ApplicationStatusId])
);


CREATE TABLE [StaffingManager].[PositionTag] (
	[PositionId]		INT		         CONSTRAINT [DF__PositionTag_PositionId] DEFAULT ((0)) NOT NULL,
	[TagId]				INT				 CONSTRAINT [DF__PositionTag_TagId] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [FK_PositionTag_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [StaffingManager].[Position] ([PositionId]),
	CONSTRAINT [FK_PositionTag_TagId] FOREIGN KEY ([TagId]) REFERENCES [StaffingManager].[Tag] ([TagId])
);


CREATE TABLE [StaffingManager].[PositionTag] (
	[PositionId]		INT		         CONSTRAINT [DF_PositionTag_Position] DEFAULT ((0)) NOT NULL,
	[TagId]				INT				 CONSTRAINT [DF_PositionTag_Tag] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [FK_PositionTag_Position] FOREIGN KEY ([PositionId]) REFERENCES [StaffingManager].[Position] ([PositionId]),
	CONSTRAINT [FK_PositionTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [StaffingManager].[Tag] ([TagId])
);


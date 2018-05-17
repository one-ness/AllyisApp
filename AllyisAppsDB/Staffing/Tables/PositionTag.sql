CREATE TABLE [Staffing].[PositionTag] (
	[PositionId]		INT		         CONSTRAINT [DF_PositionTag_Position] DEFAULT ((0)) NOT NULL,
	[TagId]				INT				 CONSTRAINT [DF_PositionTag_Tag] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [FK_PositionTag_Position] FOREIGN KEY ([PositionId]) REFERENCES [Staffing].[Position] ([PositionId]) ON DELETE CASCADE,
	CONSTRAINT [FK_PositionTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Lookup].[Tag] ([TagId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PositionId_TagId]
	ON [Staffing].[PositionTag] ([PositionId], [TagId])

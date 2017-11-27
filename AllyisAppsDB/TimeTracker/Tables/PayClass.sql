CREATE TABLE [Hrm].[PayClass] (
    [PayClassId]		INT				IDENTITY (1, 1) NOT NULL,
    [PayClassName]      NVARCHAR (32)	NOT NULL,
	[BuiltInPayClassId]	TINYINT			NOT NULL DEFAULT 0,		
    [OrganizationId]	INT				NOT NULL,
    CONSTRAINT [PK_PayClass] PRIMARY KEY NONCLUSTERED ([PayClassId] ASC),
    CONSTRAINT [FK_PayClass_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);






GO
CREATE CLUSTERED INDEX [IX_PayClass_OrganizationId]
    ON [Hrm].[PayClass]([OrganizationId] ASC);


GO

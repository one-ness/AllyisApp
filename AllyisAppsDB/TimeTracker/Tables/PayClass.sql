CREATE TABLE [TimeTracker].[PayClass] (
    [PayClassId]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (32) NOT NULL,
    [OrganizationId] INT           NOT NULL,
    CONSTRAINT [PK_PayClass_Id] PRIMARY KEY NONCLUSTERED ([PayClassId] ASC),
    CONSTRAINT [FK_PayClass_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);






GO
CREATE CLUSTERED INDEX [IX_PayClass_OrganizationId]
    ON [TimeTracker].[PayClass]([OrganizationId] ASC);


GO

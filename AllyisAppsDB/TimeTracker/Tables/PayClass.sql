CREATE TABLE [TimeTracker].[PayClass] (
    [PayClassID]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [OrganizationId] INT           DEFAULT ((0)) NOT NULL,
    [CreatedUTC]     DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_PayClass_ID] PRIMARY KEY NONCLUSTERED ([PayClassID] ASC),
    CONSTRAINT [FK_PayClass_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);


GO
CREATE CLUSTERED INDEX [IX_PayClass_OrganizationId]
    ON [TimeTracker].[PayClass]([OrganizationId] ASC);


GO
CREATE TRIGGER [TimeTracker].trg_update_PayClass ON [TimeTracker].[PayClass] FOR UPDATE AS
BEGIN
UPDATE [TimeTracker].[PayClass] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [TimeTracker].[PayClass] INNER JOIN [deleted] [d] ON [PayClass].PayClassID = [d].PayClassID;
END
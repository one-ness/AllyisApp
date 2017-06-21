CREATE TABLE [TimeTracker].[TimeEntry] (
    [TimeEntryId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [Date]        DATETIME2 (0)  NOT NULL,
    [Duration]    FLOAT (53)     NOT NULL,
    [Description] NVARCHAR (120) NULL,
    [LockSaved]   BIT            DEFAULT ((0)) NOT NULL,
    [PayClassId]  INT            DEFAULT ('Regular') NOT NULL,
    [CreatedUTC]  DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC] DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([TimeEntryId] ASC),
    CONSTRAINT [FK_TimeEntry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project] ([ProjectId]),
    CONSTRAINT [FK_TimeEntry_UserId] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_TimeEntry]
    ON [TimeTracker].[TimeEntry]([UserId] ASC, [ProjectId] ASC);


GO

CREATE TRIGGER [TimeTracker].trg_update_TimeEntry ON [TimeTracker].[TimeEntry] FOR UPDATE AS
BEGIN
	UPDATE [TimeTracker].[TimeEntry] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
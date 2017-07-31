CREATE TABLE [TimeTracker].[TimeEntry] (
    [TimeEntryId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [Date]        DATE			 NOT NULL,
    [Duration]    FLOAT (53)     NOT NULL,
    [Description] NVARCHAR (128) NULL,
    [LockSaved]   BIT            DEFAULT 0 NOT NULL,
    [PayClassId]  INT            DEFAULT 1 NOT NULL,
    [CreatedUtc]  DATETIME2 (0)  NOT NULL DEFAULT getutcdate(),
    [ModifiedUtc] DATETIME2 (0)  NOT NULL DEFAULT getutcdate(),
    CONSTRAINT [PK_TimeEntry] PRIMARY KEY NONCLUSTERED ([TimeEntryId] ASC),
    CONSTRAINT [FK_TimeEntry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]),
    CONSTRAINT [FK_TimeEntry_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);






GO
CREATE NONCLUSTERED INDEX [IX_FK_TimeEntry]
    ON [TimeTracker].[TimeEntry]([UserId] ASC, [ProjectId] ASC);


GO

CREATE TRIGGER [TimeTracker].trg_update_TimeEntry ON [TimeTracker].[TimeEntry] FOR UPDATE AS
BEGIN
	UPDATE [TimeTracker].[TimeEntry] SET [ModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
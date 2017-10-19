﻿CREATE TABLE [TimeTracker].[TimeEntry] (
    [TimeEntryId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [Date]        DATE			 NOT NULL,
    [Duration]    FLOAT (53)     NOT NULL,
    [Description] NVARCHAR (128) NULL,
	[TimeEntryStatusId] INT      NOT NULL,
    [IsLockSaved] BIT            CONSTRAINT [DF_TimeEntry_IsLockSaved] DEFAULT 0 NOT NULL,
    [PayClassId]  INT            CONSTRAINT [DF_TimeEntry_PayClassId] DEFAULT 1 NOT NULL,
    [TimeEntryCreatedUtc]  DATETIME2 (0) CONSTRAINT [DF_TimeEntry_CreatedUtc] DEFAULT getutcdate() NOT NULL,
    [TimeEntryModifiedUtc] DATETIME2 (0) CONSTRAINT [DF_TimeEntry_ModifiedUtc] DEFAULT getutcdate() NOT NULL,
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
	UPDATE [TimeTracker].[TimeEntry] SET [TimeEntryModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
CREATE TABLE [TimeTracker].[TimeEntry](
	[TimeEntryId] [int] NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY(1,1),
	[UserId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Date] DATETIME2(0) NOT NULL,
	[Duration] [float] NOT NULL,
	[Description] [nvarchar](120) NULL,
	[LockSaved] [bit] NOT NULL DEFAULT 0,
	[PayClassId] INT NOT NULL DEFAULT 'Regular', 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_TimeEntry_UserId] FOREIGN KEY([UserId]) REFERENCES [Auth].[User] ([UserId]),
	CONSTRAINT [FK_TimeEntry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project] ([ProjectId])
)
GO


CREATE NONCLUSTERED INDEX [IX_FK_TimeEntry]
	ON [TimeTracker].[TimeEntry](UserId, ProjectId);
GO

CREATE TRIGGER [TimeTracker].trg_update_TimeEntry ON [TimeTracker].[TimeEntry] FOR UPDATE AS
BEGIN
	UPDATE [TimeTracker].[TimeEntry] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
GO
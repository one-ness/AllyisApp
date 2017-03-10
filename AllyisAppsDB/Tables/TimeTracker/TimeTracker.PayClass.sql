CREATE TABLE [TimeTracker].[PayClass]
(
    [PayClassID] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
	[OrganizationId] INT NOT NULL DEFAULT 0,
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_PayClass_ID] PRIMARY KEY NONCLUSTERED ([PayClassID]),
	CONSTRAINT [FK_PayClass_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId])
)
GO


GO
CREATE TRIGGER [TimeTracker].trg_update_PayClass ON [TimeTracker].[PayClass] FOR UPDATE AS
BEGIN
UPDATE [TimeTracker].[PayClass] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [TimeTracker].[PayClass] INNER JOIN [deleted] [d] ON [PayClass].PayClassID = [d].PayClassID;
END
GO

CREATE CLUSTERED INDEX [IX_PayClass_OrganizationId] ON [TimeTracker].[PayClass] ([OrganizationId])

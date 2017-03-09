CREATE TABLE [Auth].[Logging]
(
    [LoggingId] BIGINT NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY(1,1), 
    [EntityName] NVARCHAR(128) NOT NULL, 
    [UserId] INT NOT NULL,
    [Action] NVARCHAR(128) NOT NULL,
    [DateModified] DATETIME2(0) NOT NULL,
    [DataBefore] NVARCHAR(512) NULL,
    [DataAfter] NVARCHAR(512) NULL,
    CONSTRAINT [FK_Logging_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User]([UserId]),
)

GO

CREATE CLUSTERED INDEX [IX_Logging_UserId] ON [Auth].[Logging] ([UserId])

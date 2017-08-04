CREATE TABLE [Auth].[Logging] (
    [LoggingId]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [EntityName]      NVARCHAR (128) NOT NULL,
    [UserId]          INT            NOT NULL,
    [Action]          NVARCHAR (128) NOT NULL,
    [LoggingDateModifiedUtc] DATETIME2 (0) CONSTRAINT [DF_Logging_DateModifiedUtc] DEFAULT getutcdate() NOT NULL,
    [DataBefore]      NVARCHAR (512) NULL,
    [DataAfter]       NVARCHAR (512) NULL,
    CONSTRAINT [PK_Logging] PRIMARY KEY NONCLUSTERED ([LoggingId] ASC),
    CONSTRAINT [FK_Logging_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);




GO
CREATE CLUSTERED INDEX [IX_Logging_UserId]
    ON [Auth].[Logging]([UserId] ASC);


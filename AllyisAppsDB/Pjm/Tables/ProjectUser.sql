CREATE TABLE [Pjm].[ProjectUser] (
    [ProjectId]  INT           NOT NULL,
    [UserId]     INT           NOT NULL,
    [IsActive]   BIT           NOT NULL,
    [ProjectUserCreatedUtc] DATETIME2 (0) CONSTRAINT [DF_ProjectUser_CreatedUtc] DEFAULT getutcdate() NOT NULL,
    CONSTRAINT [PK_ProjectUser] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC, [UserId] ASC),
    CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProjectUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);

GO
CREATE CLUSTERED INDEX [IX_ProjectUser_ProjectId]
    ON [Pjm].[ProjectUser]([ProjectId] ASC);


GO

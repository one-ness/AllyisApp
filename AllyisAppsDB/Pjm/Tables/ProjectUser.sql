CREATE TABLE [Pjm].[ProjectUser] (
    [ProjectId]  INT           NOT NULL,
    [UserId]     INT           NOT NULL,
    [IsActive]   BIT           NOT NULL,
    [ProjectUserCreatedUtc] DATETIME2 (0) NOT NULL DEFAULT getutcdate(),
    CONSTRAINT [PK_OrganizationUser_ProjectId_UserId] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC, [UserId] ASC),
    CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]),
    CONSTRAINT [FK_ProjectUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);






GO
CREATE CLUSTERED INDEX [IX_ProjectUser_ProjectId]
    ON [Pjm].[ProjectUser]([ProjectId] ASC);


GO

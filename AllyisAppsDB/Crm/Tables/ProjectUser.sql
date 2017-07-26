CREATE TABLE [Crm].[ProjectUser] (
    [ProjectId]  INT           NOT NULL,
    [UserId]     INT           NOT NULL,
    [IsActive]   BIT           NOT NULL,
    [CreatedUtc] DATETIME2 (0) NOT NULL DEFAULT getutcdate(),
    CONSTRAINT [PK_OrganizationUser_ProjectId_UserId] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC, [UserId] ASC),
    CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project] ([ProjectId]),
    CONSTRAINT [FK_ProjectUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);






GO
CREATE CLUSTERED INDEX [IX_ProjectUser_ProjectId]
    ON [Crm].[ProjectUser]([ProjectId] ASC);


GO

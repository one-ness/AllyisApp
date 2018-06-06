CREATE TABLE [Auth].[Permission] (
    [ProductRoleId] INT NOT NULL,
    [UserActionId]  INT NOT NULL,
    [AppEntityId]   INT NOT NULL,
    [IsDenied]      BIT CONSTRAINT [DF_Permission_IsAllowed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([ProductRoleId] ASC, [UserActionId] ASC, [AppEntityId] ASC),
    CONSTRAINT [FK_Permission_ProductRole] FOREIGN KEY ([ProductRoleId]) REFERENCES [Auth].[ProductRole] ([ProductRoleId])
);








GO
CREATE NONCLUSTERED INDEX [IX_Permission]
    ON [Auth].[Permission]([UserActionId] ASC, [AppEntityId] ASC);






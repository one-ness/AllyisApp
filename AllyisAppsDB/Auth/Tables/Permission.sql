CREATE TABLE [Auth].[Permission] (
    [ProductRoleId] INT NOT NULL,
    [EntityId]      INT NOT NULL,
    [ActionId]      INT NOT NULL,
    [IsAllowed]     BIT CONSTRAINT [DF_Permission_IsAllowed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([ProductRoleId] ASC),
    CONSTRAINT [FK_Permission_ProductRole] FOREIGN KEY ([ProductRoleId]) REFERENCES [Auth].[ProductRole] ([ProductRoleId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Permission]
    ON [Auth].[Permission]([ActionId] ASC, [EntityId] ASC);


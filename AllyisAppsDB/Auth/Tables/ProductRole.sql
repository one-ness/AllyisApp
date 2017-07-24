CREATE TABLE [Auth].[ProductRole] (
    [ProductRoleId]   INT           NOT NULL,
    [ProductId]       SMALLINT      NOT NULL,
    [Name]            NVARCHAR (64) NOT NULL,
    [PermissionAdmin] BIT           CONSTRAINT [DF__ProductRo__Permi__5812160E] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__ProductR__E5D91894EF4B4E0C] PRIMARY KEY CLUSTERED ([ProductRoleId] ASC),
    CONSTRAINT [FK_ProductRole_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);



GO
CREATE NONCLUSTERED INDEX [IX_FK_ProductRole]
    ON [Auth].[ProductRole]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductRole_ProductId]
    ON [Auth].[ProductRole]([ProductId] ASC);

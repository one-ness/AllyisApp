CREATE TABLE [Auth].[ProductRole] (
    [ProductRoleId]   INT           NOT NULL,
    [ProductId]       INT           NOT NULL,
    [Name]            NVARCHAR (32) NOT NULL,
    [PermissionAdmin] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductRoleId] ASC),
    CONSTRAINT [FK_ProductRole_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ProductRole]
    ON [Auth].[ProductRole]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductRole_ProductId]
    ON [Auth].[ProductRole]([ProductId] ASC);


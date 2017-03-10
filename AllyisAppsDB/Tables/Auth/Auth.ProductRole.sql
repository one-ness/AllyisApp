CREATE TABLE [Auth].[ProductRole]
(
    [ProductRoleId] INT NOT NULL PRIMARY KEY,
    [ProductId] int NOT NULL,
    [Name] NVARCHAR(32) NOT NULL,
    [PermissionAdmin] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ProductRole_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product]([ProductId]),
    
)

GO

CREATE INDEX [IX_ProductRole_ProductId] ON [Auth].[ProductRole] ([ProductId])

GO

CREATE NONCLUSTERED INDEX [IX_FK_ProductRole]
	ON [Auth].[ProductRole](ProductId);
GO
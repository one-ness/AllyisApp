CREATE TABLE [Auth].[ProductRole] (
    [ProductRoleId]        INT            IDENTITY (101, 1) NOT NULL,
    [ProductId]            INT            NOT NULL,
    [ProductRoleShortName] NVARCHAR (32)  NOT NULL,
    [ProductRoleFullName]  NVARCHAR (128) NOT NULL,
    [OrgOrSubId]           INT            NOT NULL,
    [BuiltInProductRoleId] INT            CONSTRAINT [DF_ProductRole_IsBuiltin] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProductRole] PRIMARY KEY CLUSTERED ([ProductRoleId] ASC),
    CONSTRAINT [FK_ProductRole_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);





GO
CREATE NONCLUSTERED INDEX [IX_ProductRole]
    ON [Auth].[ProductRole]([ProductId] ASC, [OrgOrSubId] ASC);


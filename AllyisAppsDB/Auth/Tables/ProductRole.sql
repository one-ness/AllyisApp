CREATE TABLE [Auth].[ProductRole] (
    [ProductRoleId] INT           NOT NULL,
    [ProductId]     INT           NOT NULL,
    [Name]          NVARCHAR (64) NOT NULL,
    [OrganizationId] INT NULL, 
    CONSTRAINT [PK_ProductRole] PRIMARY KEY CLUSTERED ([ProductRoleId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_ProductRole_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId]),
	CONSTRAINT [FK_ProductRole_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);





GO



GO


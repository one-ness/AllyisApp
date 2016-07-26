CREATE TABLE [Billing].[Product]
(
    [ProductId] INT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(32) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [AreaUrl] NVARCHAR(32) NOT NULL
)
GO

CREATE INDEX [IX_Product_Name] ON [Billing].[Product]([Name])

GO

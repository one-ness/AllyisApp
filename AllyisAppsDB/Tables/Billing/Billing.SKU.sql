CREATE TABLE [Billing].[Sku]
(
	[SkuId] INT NOT NULL PRIMARY KEY,
	[ProductId] INT NOT NULL,
	[Name] NVARCHAR(128) NOT NULL,
    [CostPerBlock] MONEY NOT NULL,
    [UserLimit] INT NOT NULL, 
    [BillingFrequency] NVARCHAR(50) NOT NULL DEFAULT 'Monthly', 
    [Tier] NVARCHAR(50) NOT NULL, 
    [EntityName] NVARCHAR(50) NOT NULL DEFAULT 'User', 
    [BlockSize] INT NOT NULL, 
    [PromoCostPerBlock] MONEY NULL, 
    [PromoDeadline] DATETIME2(0) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Skus_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product]([ProductId]),
)
GO

CREATE INDEX [IX_Sku_Name] ON [Billing].[Sku]([Name])
GO
CREATE NONCLUSTERED INDEX [IX_FK_SKU]
	ON [Billing].[Sku](ProductId);
GO
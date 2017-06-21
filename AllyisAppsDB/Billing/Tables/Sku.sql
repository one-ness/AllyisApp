CREATE TABLE [Billing].[Sku] (
    [SkuId]             INT            NOT NULL,
    [ProductId]         INT            NOT NULL,
    [Name]              NVARCHAR (128) NOT NULL,
    [CostPerBlock]      MONEY          NOT NULL,
    [UserLimit]         INT            NOT NULL,
    [BillingFrequency]  NVARCHAR (50)  DEFAULT ('Monthly') NOT NULL,
    [Tier]              NVARCHAR (50)  NOT NULL,
    [EntityName]        NVARCHAR (50)  DEFAULT ('User') NOT NULL,
    [BlockSize]         INT            NOT NULL,
    [PromoCostPerBlock] MONEY          NULL,
    [PromoDeadline]     DATETIME2 (0)  NULL,
    [IsActive]          BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([SkuId] ASC),
    CONSTRAINT [FK_Skus_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_SKU]
    ON [Billing].[Sku]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Sku_Name]
    ON [Billing].[Sku]([Name] ASC);


CREATE TABLE [Billing].[Sku] (
    [SkuId]             SMALLINT            NOT NULL,
    [ProductId]         SMALLINT            NOT NULL,
    [Name]              NVARCHAR (64) NOT NULL,
    [CostPerBlock]      MONEY          NOT NULL,
	[BlockBasedOn]		TINYINT   NOT NULL,
    [UserLimit]         INT            NOT NULL DEFAULT 0,
    [BillingFrequency]  TINYINT  DEFAULT 1 NOT NULL,
    
    [BlockSize]         INT            NOT NULL DEFAULT 1,
	[IsActive]          BIT            DEFAULT ((1)) NOT NULL,
	[Description] NVARCHAR(512) NULL,
    [PromoCostPerBlock] MONEY          NULL,
    [PromoDeadline]     DATETIME2 (0)  NULL 
    PRIMARY KEY CLUSTERED ([SkuId] ASC),
    CONSTRAINT [FK_Skus_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_SKU]
    ON [Billing].[Sku]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Sku_Name]
    ON [Billing].[Sku]([Name] ASC);


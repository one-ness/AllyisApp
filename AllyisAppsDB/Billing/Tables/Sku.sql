CREATE TABLE [Billing].[Sku] (
    [SkuId]             INT            NOT NULL,
    [ProductId]         INT            NOT NULL,
    [SkuName]              NVARCHAR (64)  NOT NULL,
    [CostPerBlock]      MONEY          NOT NULL,
    [BlockBasedOn]      TINYINT        NOT NULL,
    [UserLimit]         INT            CONSTRAINT [DF__Sku__UserLimit] DEFAULT ((0)) NOT NULL,
    [BillingFrequency]  TINYINT        CONSTRAINT [DF__Sku__BillingFrequency] DEFAULT ((1)) NOT NULL,
    [BlockSize]         INT            CONSTRAINT [DF__Sku__BlockSize] DEFAULT ((1)) NOT NULL,
    [IsActive]          BIT            CONSTRAINT [DF__Sku__IsActive] DEFAULT ((1)) NOT NULL,
    [Description]       NVARCHAR (512) NULL,
    [PromoCostPerBlock] MONEY          NULL,
    [PromoDeadline]     DATETIME2 (0)  NULL,
    [IconUrl] NVARCHAR(512) NULL, 
    CONSTRAINT [PK_Sku] PRIMARY KEY CLUSTERED ([SkuId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_Sku_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);




GO



GO



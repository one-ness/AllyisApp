CREATE TABLE [Billing].[Sku] (
    [SkuId]             INT            NOT NULL,
    [ProductId]         INT            NOT NULL,
    [Name]              NVARCHAR (64)  NOT NULL,
    [CostPerBlock]      MONEY          NOT NULL,
    [BlockBasedOn]      TINYINT        NOT NULL,
    [UserLimit]         INT            CONSTRAINT [DF__Sku__UserLimit__6E01572D] DEFAULT ((0)) NOT NULL,
    [BillingFrequency]  TINYINT        CONSTRAINT [DF__Sku__BillingFreq__6EF57B66] DEFAULT ((1)) NOT NULL,
    [BlockSize]         INT            CONSTRAINT [DF__Sku__BlockSize__6FE99F9F] DEFAULT ((1)) NOT NULL,
    [IsActive]          BIT            CONSTRAINT [DF__Sku__IsActive__70DDC3D8] DEFAULT ((1)) NOT NULL,
    [Description]       NVARCHAR (512) NULL,
    [PromoCostPerBlock] MONEY          NULL,
    [PromoDeadline]     DATETIME2 (0)  NULL,
    CONSTRAINT [PK_Sku] PRIMARY KEY CLUSTERED ([SkuId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_Sku_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);




GO



GO



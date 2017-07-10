CREATE TABLE [Billing].[Product] (
    [ProductId]   SMALLINT            NOT NULL,
    [Name]        NVARCHAR (32)  NOT NULL,
    [Description] NVARCHAR (512) NULL,
    [IsActive]    BIT            DEFAULT ((1)) NOT NULL,
    [AreaUrl]     NVARCHAR (32)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Product_Name]
    ON [Billing].[Product]([Name] ASC);


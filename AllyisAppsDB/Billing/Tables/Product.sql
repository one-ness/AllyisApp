CREATE TABLE [Billing].[Product] (
    [ProductId]   INT            NOT NULL,
    [ProductName]        NVARCHAR (32)  NOT NULL,
    [Description] NVARCHAR (128) NULL,
    [IsActive]    BIT            CONSTRAINT [DF__Product__IsActive] DEFAULT ((1)) NOT NULL,
    [AreaUrl]     NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);




GO



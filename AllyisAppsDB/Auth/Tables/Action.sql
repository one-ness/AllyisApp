CREATE TABLE [Auth].[Action] (
    [ProductId]  INT           NOT NULL,
    [ActionId]   INT           NOT NULL,
    [ActionName] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED ([ProductId] ASC, [ActionId] ASC),
    CONSTRAINT [FK_Action_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);


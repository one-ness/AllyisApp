CREATE TABLE [Billing].[BillingHistory] (
    [Date]           DATETIME2 (0)  NOT NULL,
    [Description]    NVARCHAR (MAX) NOT NULL,
    [OrganizationId] INT            NOT NULL,
    [UserId]         INT            NOT NULL,
    [SkuId]          INT            NULL,
    [CreatedUTC]     DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Date] ASC),
    CONSTRAINT [FK_BillingHistory_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_BillingHistory_Sku] FOREIGN KEY ([SkuId]) REFERENCES [Billing].[Sku] ([SkuId]),
    CONSTRAINT [FK_BillingHistory_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_BillingHistory]
    ON [Billing].[BillingHistory]([OrganizationId] ASC, [UserId] ASC, [SkuId] ASC);


GO
CREATE CLUSTERED INDEX [IX_BillingHistory_OrganizationId_UserId]
    ON [Billing].[BillingHistory]([OrganizationId] ASC, [UserId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_BillingHistory ON [Billing].[BillingHistory] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[BillingHistory] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[BillingHistory] INNER JOIN [deleted] [d] ON [BillingHistory].[Date] = [d].[Date];
END
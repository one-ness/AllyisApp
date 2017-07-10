CREATE TABLE [Billing].[Subscription] (
    [SubscriptionId]   INT           IDENTITY (1, 1) NOT NULL,
    [OrganizationId]   INT           NOT NULL,
    [SkuId]            SMALLINT           NOT NULL,
    [NumberOfUsers]    INT           NOT NULL DEFAULT 0,
    [IsActive]         BIT           DEFAULT ((1)) NOT NULL,
    [CreatedUtc]       DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUtc]      DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [SubscriptionName] NVARCHAR (32) NULL,
    [PromoExpirationDateUtc] DATETIME2(0) NULL, 
    PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_OrganizationSubscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_OrganizationSubscription_Sku] FOREIGN KEY ([SkuId]) REFERENCES [Billing].[Sku] ([SkuId])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_Subscription]
    ON [Billing].[Subscription]([OrganizationId] ASC, [SkuId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Subscription_SkuId]
    ON [Billing].[Subscription]([SkuId] ASC);


GO
CREATE CLUSTERED INDEX [IX_Subscription_OrganizationId]
    ON [Billing].[Subscription]([OrganizationId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_Subscription ON [Billing].[Subscription] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[Subscription] SET [ModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[Subscription] INNER JOIN [deleted] [d] ON [Subscription].[SubscriptionId] = [d].[SubscriptionId];
END
CREATE TABLE [Billing].[Subscription]
(
    [SubscriptionId] INT PRIMARY KEY NONCLUSTERED IDENTITY (1,1),
    [OrganizationId] INT NOT NULL,
    [SkuId] INT NOT NULL,
    [NumberOfUsers] INT NOT NULL , 
    [IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedUTC] DATETIME2(0) DEFAULT GETUTCDATE() NOT NULL,

    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_OrganizationSubscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId]),
    CONSTRAINT [FK_OrganizationSubscription_Sku] FOREIGN KEY ([SkuId]) REFERENCES [Billing].[Sku]([SkuId]),
)
GO

CREATE INDEX [IX_Subscription_SkuId] ON [Billing].[Subscription] ([SkuId])
GO

CREATE CLUSTERED INDEX [IX_Subscription_OrganizationId] ON [Billing].[Subscription] ([OrganizationId])

GO
CREATE NONCLUSTERED INDEX [IX_FK_Subscription]
	ON [Billing].[Subscription](OrganizationId, SkuId);
GO
CREATE TRIGGER [Billing].trg_update_Subscription ON [Billing].[Subscription] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[Subscription] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[Subscription] INNER JOIN [deleted] [d] ON [Subscription].[SubscriptionId] = [d].[SubscriptionId];
END
GO
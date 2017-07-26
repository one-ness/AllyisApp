CREATE TABLE [Billing].[Subscription] (
    [SubscriptionId]         INT           NOT NULL,
    [OrganizationId]         INT           NOT NULL,
    [SkuId]                  INT           IdENTITY (113969, 7) NOT NULL,
    [SubscriptionName]       NVARCHAR (64) NULL,
    [NumberOfUsers]          INT           NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [CreatedUtc]             DATETIME2 (0) NOT NULL,
    [ModifiedUtc]            DATETIME2 (0) NOT NULL,
    [PromoExpirationDateUtc] DATETIME2 (0) NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_Subscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);






GO



GO



GO
CREATE CLUSTERED INDEX [IX_Subscription_OrganizationId]
    ON [Billing].[Subscription]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Subscription]
    ON [Billing].[Subscription]([OrganizationId] ASC, [SkuId] ASC);


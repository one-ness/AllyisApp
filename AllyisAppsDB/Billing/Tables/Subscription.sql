CREATE TABLE [Billing].[Subscription] (
    [SubscriptionId]         INT           IDENTITY (113969, 7) NOT NULL,
    [OrganizationId]         INT           NOT NULL,
    [SkuId]                  INT           NOT NULL,
    [SubscriptionName]       NVARCHAR (64) NULL,
    [NumberOfUsers]          INT           NOT NULL DEFAULT ((1)),
    [IsActive]               BIT           CONSTRAINT [DF_Subscription_IsActive] DEFAULT ((1)) NOT NULL,
    [SubscriptionCreatedUtc]             DATETIME2 (0) CONSTRAINT [DF_Subscription_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [SubscriptionModifiedUtc]            DATETIME2 (0) CONSTRAINT [DF_Subscription_ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PromoExpirationDateUtc] DATETIME2 (0) NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_Subscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);



GO
CREATE CLUSTERED INDEX [IX_Subscription_OrganizationId]
    ON [Billing].[Subscription]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Subscription]
    ON [Billing].[Subscription]([OrganizationId] ASC, [SkuId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_NumberOfUsers_on_insert ON [Billing].[SubscriptionUser] FOR INSERT AS
BEGIN
	UPDATE [Billing].[Subscription] SET [NumberOfUsers] = (SELECT COUNT(*) FROM [Billing].[SubscriptionUser] [s] INNER JOIN  [inserted] [i] ON [s].[SubscriptionId] = [i].[SubscriptionId]);
END


GO
CREATE TRIGGER [Billing].trg_update_NumberOfUsers_on_delete ON [Billing].[SubscriptionUser] FOR DELETE AS
BEGIN
	UPDATE [Billing].[Subscription] SET [NumberOfUsers] = (SELECT COUNT(*) FROM [Billing].[SubscriptionUser] [s] INNER JOIN  [deleted] [d] ON [s].[SubscriptionId] = [d].[SubscriptionId]);
END


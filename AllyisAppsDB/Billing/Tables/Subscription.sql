﻿CREATE TABLE [Billing].[Subscription] (
    [SubscriptionId]         INT           IDENTITY (113969, 7) NOT NULL,
    [OrganizationId]         INT           NOT NULL,
    [SkuId]                  INT           NOT NULL,
    [SubscriptionName]       NVARCHAR (64) NULL,
    [UserCount]              AS            ([Billing].[GetSubscriptionUserCount]([SubscriptionId])),
    [SubscriptionStatus]     INT           CONSTRAINT [DF_Subscription_IsActive] DEFAULT ((1)) NOT NULL,
    [SubscriptionCreatedUtc] DATETIME2 (0) CONSTRAINT [DF_Subscription_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PromoExpirationDateUtc] DATETIME2 (0) NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_Subscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);





Go 


GO
CREATE CLUSTERED INDEX [IX_Subscription_OrganizationId]
    ON [Billing].[Subscription]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Subscription]
    ON [Billing].[Subscription]([OrganizationId] ASC, [SkuId] ASC);

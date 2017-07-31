CREATE TABLE [Billing].[SubscriptionUser] (
    [SubscriptionId] INT           NOT NULL,
    [UserId]         INT           NOT NULL,
    [ProductRoleId]  INT           NOT NULL,
    [CreatedUtc]     DATETIME2 (0) CONSTRAINT [DF_SubscriptionUser_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SubscriptionUser] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC, [UserId] ASC),
    CONSTRAINT [FK_SubscriptionUser_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]),
    CONSTRAINT [FK_SubscriptionUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]) ON DELETE CASCADE
);






GO



GO



GO



GO

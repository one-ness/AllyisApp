CREATE TABLE [Billing].[SubscriptionUser] (
    [SubscriptionId] INT           NOT NULL,
    [UserId]         INT           NOT NULL,
    [ProductRoleId]  INT           NOT NULL,
    [CreatedUTC]     DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Sku_USER] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC, [UserId] ASC),
    CONSTRAINT [FK_SubscriptionUser_ProductRole] FOREIGN KEY ([ProductRoleId]) REFERENCES [Auth].[ProductRole] ([ProductRoleId]),
    CONSTRAINT [FK_SubscriptionUser_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]),
    CONSTRAINT [FK_SubscriptionUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]) ON DELETE CASCADE,
    CONSTRAINT [SubscriptionUserNoDuplicates] UNIQUE NONCLUSTERED ([UserId] ASC, [SubscriptionId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_SubscriptionUser]
    ON [Billing].[SubscriptionUser]([SubscriptionId] ASC, [UserId] ASC, [ProductRoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SubscriptionUser_UserId]
    ON [Billing].[SubscriptionUser]([UserId] ASC);


GO
CREATE CLUSTERED INDEX [IX_SubscriptionUser_SubscriptionId]
    ON [Billing].[SubscriptionUser]([SubscriptionId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_SubscriptionUser ON [Billing].[SubscriptionUser] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[SubscriptionUser] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[SubscriptionUser] INNER JOIN [deleted] [d] ON [SubscriptionUser].[SubscriptionId] = [d].[SubscriptionId] AND [SubscriptionUser].[UserId] = [d].[UserId];
END
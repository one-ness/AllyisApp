CREATE TABLE [Billing].[SubscriptionUser] (
    [SubscriptionId] INT           NOT NULL,
    [UserId]         INT           NOT NULL,
    [ProductRoleId]  INT           NOT NULL,
    [CreatedUtc]     DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_SubscriptionUser] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC, [UserId] ASC),
    CONSTRAINT [FK_SubscriptionUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]) ON DELETE CASCADE
);




GO



GO



GO



GO

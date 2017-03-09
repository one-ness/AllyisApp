CREATE TABLE [Auth].[InvitationSubRole]
(
    [InvitationId] INT PRIMARY KEY, 
    [SubscriptionId] INT NOT NULL, 
    [ProductRoleId] INT NOT NULL, 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_InvitationSubRole_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Auth].[Invitation]([InvitationId]),
    CONSTRAINT [FK_InvitationSubRole_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Billing].[Subscription]([SubscriptionId]),
    CONSTRAINT [FK_InvitationSubRole_ProductRoleId] FOREIGN KEY ([ProductRoleId]) REFERENCES [Auth].[ProductRole]([ProductRoleId]),

)

GO

CREATE INDEX [IX_InvitationSubRole_SubscriptionId] ON [Auth].[InvitationSubRole] ([SubscriptionId])
GO
CREATE TRIGGER [Auth].trg_update_InvitationSubRole ON [Auth].[InvitationSubRole] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[InvitationSubRole] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[InvitationSubRole] INNER JOIN [deleted] [d] ON [InvitationSubRole].[InvitationId] = [d].[InvitationId];
END
GO
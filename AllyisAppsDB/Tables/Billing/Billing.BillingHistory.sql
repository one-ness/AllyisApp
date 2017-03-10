CREATE TABLE [Billing].[BillingHistory]
(
    [Date] DATETIME2(0) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [OrganizationId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [SkuId] INT NULL, 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_BillingHistory_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId]), 
    CONSTRAINT [FK_BillingHistory_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User]([UserId]), 
    CONSTRAINT [FK_BillingHistory_Sku] FOREIGN KEY ([SkuId]) REFERENCES [Billing].[Sku]([SkuId])

)

GO

CREATE CLUSTERED INDEX [IX_BillingHistory_OrganizationId_UserId] ON [Billing].[BillingHistory] ([OrganizationId], [UserId])
GO
CREATE NONCLUSTERED INDEX [IX_FK_BillingHistory]
	ON [Billing].[BillingHistory](OrganizationId, UserId, SkuId);
GO
CREATE TRIGGER [Billing].trg_update_BillingHistory ON [Billing].[BillingHistory] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[BillingHistory] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[BillingHistory] INNER JOIN [deleted] [d] ON [BillingHistory].[Date] = [d].[Date];
END
GO
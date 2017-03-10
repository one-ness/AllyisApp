CREATE TABLE [Billing].[StripeCustomerSubscriptionPlan]
(
    [StripeTokenCustId] NVARCHAR(50) NOT NULL , 
    [StripeTokenSubId] NCHAR(50) NOT NULL PRIMARY KEY, 
    [NumberOfUsers] int NOT NULL, 
    [Price] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [OrganizationId] INT NOT NULL, 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_CustomerSubscriptionPlan_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product]([ProductId]), 
    CONSTRAINT [FK_CustomerSubscriptionPlan_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId])
    ) 

    GO
CREATE NONCLUSTERED INDEX [IX_FK_StripeCustomerSubscriptionPlan]
	ON [Billing].[StripeCustomerSubscriptionPlan](ProductId, OrganizationId);
GO
CREATE TRIGGER [Billing].trg_update_CustomerSubscriptionPlan ON [Billing].[StripeCustomerSubscriptionPlan] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[StripeCustomerSubscriptionPlan] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[StripeCustomerSubscriptionPlan] INNER JOIN [deleted] [d] ON [StripeCustomerSubscriptionPlan].[StripeTokenSubId] = [d].[StripeTokenSubId];
END
GO
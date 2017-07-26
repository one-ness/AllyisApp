CREATE TABLE [Billing].[StripeCustomerSubscriptionPlan] (
    [StripeTokenCustId] NVARCHAR (50) NOT NULL,
    [StripeTokenSubId]  NCHAR (50)    NOT NULL,
    [NumberOfUsers]     INT           NOT NULL,
    [Price]             INT           NOT NULL,
    [ProductId]         INT           NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [OrganizationId]    INT           NOT NULL,
    [CreatedUtc]        DATETIME2 (0) CONSTRAINT [DF__StripeCus__Creat__6C190EBB] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUtc]       DATETIME2 (0) CONSTRAINT [DF__StripeCus__Modif__6D0D32F4] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK__StripeCu__902596C6930887E0] PRIMARY KEY CLUSTERED ([StripeTokenSubId] ASC),
    CONSTRAINT [FK_CustomerSubscriptionPlan_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_CustomerSubscriptionPlan_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_StripeCustomerSubscriptionPlan]
    ON [Billing].[StripeCustomerSubscriptionPlan]([ProductId] ASC, [OrganizationId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_CustomerSubscriptionPlan ON [Billing].[StripeCustomerSubscriptionPlan] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[StripeCustomerSubscriptionPlan] SET [ModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[StripeCustomerSubscriptionPlan] INNER JOIN [deleted] [d] ON [StripeCustomerSubscriptionPlan].[StripeTokenSubId] = [d].[StripeTokenSubId];
END
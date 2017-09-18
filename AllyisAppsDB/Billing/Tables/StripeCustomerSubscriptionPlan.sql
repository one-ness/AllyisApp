CREATE TABLE [Billing].[StripeCustomerSubscriptionPlan] (
    [StripeTokenCustId] NVARCHAR (50) NOT NULL,
    [StripeTokenSubId]  NCHAR (50)    NOT NULL,
    [NumberOfUsers]     INT           NOT NULL,
    [Price]             INT           NOT NULL,
    [ProductId]         INT           NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [OrganizationId]    INT           NOT NULL,
    [StripeCustomerSubscriptionPlanCreatedUtc]        DATETIME2 (0) CONSTRAINT [DF__StripeCustomerSubscriptionPlan__CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [StripeCustomerSubscriptionPlanModifiedUtc]       DATETIME2 (0) CONSTRAINT [DF__StripeCustomerSubscriptionPlan__ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK__StripeCustomerSubscriptionPlan] PRIMARY KEY CLUSTERED ([StripeTokenSubId] ASC),
    CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_StripeCustomerSubscriptionPlan]
    ON [Billing].[StripeCustomerSubscriptionPlan]([ProductId] ASC, [OrganizationId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_CustomerSubscriptionPlan ON [Billing].[StripeCustomerSubscriptionPlan] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[StripeCustomerSubscriptionPlan] SET [StripeCustomerSubscriptionPlanModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[StripeCustomerSubscriptionPlan] INNER JOIN [deleted] [d] ON [StripeCustomerSubscriptionPlan].[StripeTokenSubId] = [d].[StripeTokenSubId];
END
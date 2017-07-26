CREATE TABLE [Billing].[StripeOrganizationCustomer] (
    [OrganizationId]    INT           NOT NULL,
    [StripeTokenCustId] NVARCHAR (50) NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [CreatedUtc]        DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUtc]       DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([StripeTokenCustId] ASC),
    CONSTRAINT [FK_OrganizationCustomer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_StripeOrganizationCustomer]
    ON [Billing].[StripeOrganizationCustomer]([OrganizationId] ASC);


GO
CREATE CLUSTERED INDEX [IX_OrganizationCustomer_OrganizationId]
    ON [Billing].[StripeOrganizationCustomer]([OrganizationId] ASC);


GO
CREATE TRIGGER [Billing].trg_update_OrganizationCustomer ON [Billing].[StripeOrganizationCustomer] FOR UPDATE AS
BEGIN
	UPDATE [Billing].[StripeOrganizationCustomer] SET [ModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[StripeOrganizationCustomer] INNER JOIN [deleted] [d] ON [StripeOrganizationCustomer].[StripeTokenCustId] = [d].[StripeTokenCustId];
END
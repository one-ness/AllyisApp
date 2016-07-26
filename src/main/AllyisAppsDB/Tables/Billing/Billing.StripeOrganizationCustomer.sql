CREATE TABLE [Billing].[StripeOrganizationCustomer]
(
	OrganizationId INT NOT NULL, 
	StripeTokenCustId NVARCHAR(50) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [IsActive] BIT NOT NULL, 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_OrganizationCustomer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId])
)


GO

CREATE CLUSTERED INDEX [IX_OrganizationCustomer_OrganizationId] ON [Billing].[StripeOrganizationCustomer] ([OrganizationId])

GO
CREATE TRIGGER [Billing].trg_update_OrganizationCustomer ON [Billing].[StripeOrganizationCustomer] FOR UPDATE AS
BEGIN
	UPDATE [Billing].[StripeOrganizationCustomer] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Billing].[StripeOrganizationCustomer] INNER JOIN [deleted] [d] ON [StripeOrganizationCustomer].[StripeTokenCustId] = [d].[StripeTokenCustId];
END
GO
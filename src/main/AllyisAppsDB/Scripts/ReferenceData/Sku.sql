-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [UserLimit], [BillingFrequency], [Tier], [BlockSize], [IsActive])
VALUES (1, 1, 'TimeTracker', 0.00, 5, 'Free', 'Free', 5, 1)

INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [UserLimit], [BillingFrequency], [Tier], [BlockSize], [IsActive])
VALUES (2, 1, 'TimeTracker - Basic', 5.00, 0, 'Monthly', 'Basic', 5, 1)


-- Consulting Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [UserLimit], [BillingFrequency], [Tier], [BlockSize], [IsActive])
VALUES (3, 2, 'Consulting', 0.00, 5, 'Free', 'Free', 5, 0)

INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [UserLimit], [BillingFrequency], [Tier], [BlockSize], [IsActive])
VALUES (4, 2, 'Consulting - Basic', 5.00, 0, 'Monthly', 'Basic', 5, 0)

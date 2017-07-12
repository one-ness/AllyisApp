-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (1, 1, 'Time Tracker', 5.95, 1, 'Time Tracker Description')

INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (4, 1, 'Time Tracker Pro', 12.95, 1, 'Time Tracker Pro Description')

-- Expense Tracker Skus (set them to inactive for now) --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (2, 2, 'Expense Tracker', 5.95, 2, 'Expense Tracker Description')

INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (3, 2, 'Expense Tracker Pro', 10.95, 2, 'Expense Tracker Pro Description')

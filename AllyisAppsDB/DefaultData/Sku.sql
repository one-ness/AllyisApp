-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn])
VALUES (1, 1, 'Time Tracker', 5.95, 1)

-- Expense Tracker Skus (set them to inactive for now) --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn])
VALUES (2, 2, 'Expense Tracker', 5.95, 2)
Update [Billing].[Sku] set [IsActive] = 0 where [SkuId] = 2
go


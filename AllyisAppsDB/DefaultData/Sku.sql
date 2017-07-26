-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (1, 1, 'Time Tracker', 5.95, 1, 'Easily track the time spent by your employees in various projects. Add unlimited number of users. Only $5.95 per monthly timesheet!')

-- INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
-- VALUES (4, 1, 'Time Tracker Pro', 10.95, 1, 'Everything in the Basic subscription plus, Customer and Project reports, Integration with purchase orders and more! Only $10.95 per monthly timesheet!')

 -- Expense Tracker Skus (set them to inactive for now) --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [IsActive], [Description])
VALUES (2, 2, 'Expense Tracker', 7.95, 2, 0, 'Effortlessly track the expenses incurred by your employees. Add unlimited number of users. Only $7.95 per submitted expense report!')

--INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
--VALUES (3, 2, 'Expense Tracker Pro', 12.95, 2, 'Everything in the Basic subscription plus, Chart of Accounts, Reports and Analysis, Integration with major Financial Institutions and more! Only $12.95 per submitted expense report!')

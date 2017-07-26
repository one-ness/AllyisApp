﻿-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (200001, 200000, 'Time Tracker', 1.99, 1, 'Easily track the time spent by your employees in various projects. Add unlimited number of users. Pay monthly, only for each submitted timesheet!')

-- Expense Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (300001, 300000, 'Expense Tracker', 5.99, 2, 'Effortlessly track the expenses incurred by your employees. Add unlimited number of users. Pay monthly, only for each submitted expense report!')

-- Staffing Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [Name], [CostPerBlock], [BlockBasedOn], [Description])
VALUES (400001, 400000, 'Staffing Manager', 24.99, 2, 'Manage your job positions, candidates, interviews and offers seamlessly in one place. Add unlimited number of users. Pay monthly, only for each position or interview!')

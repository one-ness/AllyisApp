-- Time Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [SkuName], [CostPerBlock], [BlockBasedOn], [Description],[IconUrl])
VALUES (200001, 200000, 'Time Tracker', 1.99, 1, 'Easily track the time spent by your employees in various projects. Add unlimited number of users. Pay per timesheet submitted, billed monthly! (Minimum payment of $5.00 per month)'
, 'Content/TimeTracker/icons/TimeTracker.png')

-- Expense Tracker Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [SkuName], [CostPerBlock], [BlockBasedOn], [Description], [IconUrl])
VALUES (300001, 300000, 'Expense Tracker', 1.99, 1, 'Effortlessly track the expenses incurred by your employees. Add unlimited number of users. Pay per expense report submitted, billed monthly! (Minimum payment of $5.00 per month)'
, 'Content/ExpenseTracker/icons/ExpenseTracker.png')

-- Staffing Skus --
INSERT INTO [Billing].[Sku] ([SkuId], [ProductId], [SkuName], [CostPerBlock], [BlockBasedOn], [Description],[IconUrl])
VALUES (400001, 400000, 'Staffing Manager', 1.99, 1, 'Manage your job positions, candidates, interviews and offers seamlessly in one place. Add unlimited number of users. Pay per job position created, billed monthly! (Minimum payment of $5.00 per month)'
,'Content/StaffingManager/icons/StaffingManager.png')

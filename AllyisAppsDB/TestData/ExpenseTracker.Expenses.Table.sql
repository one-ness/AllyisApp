Print 'Expenses'

--When creating a Expense Item, it needs to have a corresponding Expense Report. [AccountId] must match [SubmittedById] and the  [ExpenseReportId] for both must match as well. Other values can be any valid column value.

--Entries must be place between their respective tables SET IDENTITY_INSERT statments, otherwise an error will be thrown.

SET IDENTITY_INSERT [Expense].[ExpenseItem] ON
--Expense Item Test Data.
INSERT [Expense].[ExpenseItem] ([AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc], [ExpenseItemId],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES (111119, 435567, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 356777,  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));

SET IDENTITY_INSERT [Expense].[ExpenseItem] OFF

SET IDENTITY_INSERT [Expense].[ExpenseReport] ON
--Expense Report Test Data
INSERT [Expense].[ExpenseReport] ([SubmittedById], [ExpenseReportId], [OrganizationId], [ReportDate], [ExpenseReportCreatedUtc], [ExpenseReportModifiedUtc], [BusinessJustification], [ReportStatus], [ReportTitle]) VALUES ( 111119, 435567, 112559, CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 'We really need basic office supplies', 3, 'Basic Suplies');

SET IDENTITY_INSERT [Expense].[ExpenseItem] OFF
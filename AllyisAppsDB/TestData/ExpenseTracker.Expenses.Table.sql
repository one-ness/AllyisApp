Print 'Expenses'

SET IDENTITY_INSERT [Expense].[ExpenseReport] OFF
SET IDENTITY_INSERT [Expense].[ExpenseReport] ON
--Expense Report Test Data
INSERT [Expense].[ExpenseReport] ([SubmittedById], [ExpenseReportId], [OrganizationId], [ReportDate], [ExpenseReportCreatedUtc], [ExpenseReportModifiedUtc], [BusinessJustification], [ReportStatus], [ReportTitle]) VALUES ( 111119, 435567, 112559, CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 'We really need basic office supplies', 3, 'Basic Suplies');
INSERT [Expense].[ExpenseReport] ([SubmittedById], [ExpenseReportId], [OrganizationId], [ReportDate], [ExpenseReportCreatedUtc], [ExpenseReportModifiedUtc], [BusinessJustification], [ReportStatus], [ReportTitle]) VALUES ( 111119, 435568, 112559, CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 'We really need basic office supplies', 3, 'Basic Suplies');


SET IDENTITY_INSERT [Expense].[ExpenseReport] OFF

SET IDENTITY_INSERT [Expense].[ExpenseItem] OFF
SET IDENTITY_INSERT [Expense].[ExpenseItem] ON
--Expense Item Test Data.
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356777, 111119, 435567, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356778, 111119, 435567, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356779, 111119, 435567, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356780, 111119, 435567, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356760, 111119, 435568, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356761, 111119, 435568, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356762, 111119, 435568, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));
INSERT [Expense].[ExpenseItem] ([ExpenseItemId], [AccountId], [ExpenseReportId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc],  [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES ( 356763, 111119, 435568, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2),  1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));

SET IDENTITY_INSERT [Expense].[ExpenseItem] OFF


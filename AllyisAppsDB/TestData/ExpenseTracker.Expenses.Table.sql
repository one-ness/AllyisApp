Print 'TimeEntry'

--Expense Item Test Data
INSERT [Expense].[ExpenseItem] ([AccountId], [Amount], [ExpenseItemCreatedUtc], [ExpenseItemModifiedUtc], [ExpenseItemId], [ExpenseReportId], [IsBillableToCustomer], [ItemDescription], [TransactionDate]) VALUES (111119, 437.98, CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 356777, 435567, 1, 'Basic Supply Expenses.', CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2));

--Expense Report Test Data
INSERT [Expense].[ExpenseReport] ([ExpenseReportId], [OrganizationId], [SubmittedById], [ReportDate], [ExpenseReportCreatedUtc], [ExpenseReportModifiedUtc], [BusinessJustification], [ReportStatus], [ReportTitle]) VALUES (435567,112559, 111119, CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), CAST(N'2015-12-16 17:42:24.0000000' AS DateTime2), CAST(N'2015-09-18 00:00:00.0000000' AS DateTime2), 'We really need basic office supplies', 3, 'Basic Suplies');

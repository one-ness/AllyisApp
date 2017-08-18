CREATE PROCEDURE [Expense].[GetExpenseItemsByExpenseReportId]
	@reportId INT
AS

BEGIN
	SELECT [ExpenseItemId],
	[ExpenseItemName],
	[ItemDescription],
	[TransactionDate],
	[Amount],
	[ExpenseReportId],
	[AccountId],
	[AccountTypeId],
	[IsBillableToCustomer],
	[ExpenseItemCreatedUtc],
	[ExpenseItemModifiedUtc]
	FROM [Expense].[ExpenseItem] AS [EI]
	WHERE [EI].[ExpenseReportId] = @reportId

END

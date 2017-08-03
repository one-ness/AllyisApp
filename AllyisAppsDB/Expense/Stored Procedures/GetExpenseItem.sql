CREATE PROCEDURE [Expense].[GetExpenseItemsByExpenseItemId]
	@expenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseItemId],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[AccountId],
		[IsBillableToCustomer],
		[ExpenseItemCreatedUtc],
		[ExpenseItemModifiedUtc]
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE ExpenseItemId = @expenseItemId
END
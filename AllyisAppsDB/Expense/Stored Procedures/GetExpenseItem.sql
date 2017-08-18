CREATE PROCEDURE [Expense].[GetExpenseItemsByExpenseItemId]
	@expenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseItemId],
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
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE ExpenseItemId = @expenseItemId
END
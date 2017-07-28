CREATE PROCEDURE [Expense].[GetExpenseItemsByExpenseItemId]
	@ExpenseItemId INT
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
		[CreatedUtc],
		[ModifiedUtc]
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE ExpenseItemId = @ExpenseItemId
END
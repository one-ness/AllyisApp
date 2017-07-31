CREATE PROCEDURE [Expense].[GetExpenseItemsByAccountId]
	@accountId INT
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
	WHERE AccountId = @accountId
END
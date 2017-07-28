CREATE PROCEDURE [Expense].[GetExpenseItemsByAccountId]
	@AccountId INT
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
	WHERE AccountId = @AccountId
END
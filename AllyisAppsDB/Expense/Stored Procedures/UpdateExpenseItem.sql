CREATE PROCEDURE [Expense].[UpdateExpenseItem]
	@expenseItemId INT,
	@itemDescription NVARCHAR(100),
	@transactionDate DATETIME2(0),
	@amount DECIMAL(18, 2),
	@expenseReportId INT,
	@isBillableToCustomer BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Expense].[ExpenseItem]
	SET
		[ItemDescription] = @itemDescription,
		[TransactionDate] = @transactionDate,
		[Amount] = @amount,
		[ExpenseReportId] = @expenseReportId,
		[IsBillableToCustomer] = @isBillableToCustomer
	WHERE [ExpenseItem].[ExpenseItemId] = @expenseItemId;
END


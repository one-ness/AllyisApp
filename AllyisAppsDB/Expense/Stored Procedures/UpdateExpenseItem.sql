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

	INSERT INTO [Expense].[ExpenseItem]
		([ExpenseItemId],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer])
	VALUES (@expenseItemId,
		@itemDescription,
		@transactionDate,
		@amount,
		@expenseReportId,
		@isBillableToCustomer);
END


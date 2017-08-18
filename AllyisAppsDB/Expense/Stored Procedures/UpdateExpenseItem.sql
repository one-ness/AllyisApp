CREATE PROCEDURE [Expense].[UpdateExpenseItem]
	@expenseItemId INT,
	@expenseItemName NVARCHAR(60),
	@itemDescription NVARCHAR(100),
	@transactionDate DATETIME2(0),
	@amount DECIMAL(18, 2),
	@expenseReportId INT,
	@isBillableToCustomer BIT,
	@account NVARCHAR (60)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseItem]
		([ExpenseItemId],
		[ExpenseItemName],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer],
		[Account])
	VALUES (@expenseItemId,
		@expenseItemName,
		@itemDescription,
		@transactionDate,
		@amount,
		@expenseReportId,
		@isBillableToCustomer,
		@account);
END


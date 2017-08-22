CREATE PROCEDURE [Expense].[UpdateExpenseItem]
	@expenseItemName NVARCHAR(60),
	@itemDescription NVARCHAR(100),
	@transactionDate DATETIME2(0),
	@amount DECIMAL(18, 2),
	@expenseReportId INT,
	@isBillableToCustomer BIT,
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseItem]
		([ExpenseItemName],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer],
		[AccountId])
	VALUES (@expenseItemName,
		@itemDescription,
		@transactionDate,
		@amount,
		@expenseReportId,
		@isBillableToCustomer,
		@accountId);
END


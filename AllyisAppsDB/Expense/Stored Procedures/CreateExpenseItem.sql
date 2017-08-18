CREATE PROCEDURE [Expense].[CreateExpenseItem]
	@itemDescription NVARCHAR(100),
	@itemExpenseName NVARCHAR(60),
	@transactionDate DATETIME2(0),
	@amount DECIMAL(18, 2),
	@expenseReportId INT,
	@isBillableToCustomer BIT,
	@accountId INT,
	@account NVARCHAR(60)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseItem]
		([ItemDescription],
		[ExpenseItemName],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer],
		[AccountId],
		[Account])
	VALUES (@itemDescription,
		@itemExpenseName,
		@transactionDate,
		@amount,
		@expenseReportId,
		@isBillableToCustomer,
		@accountId,
		@account);
END
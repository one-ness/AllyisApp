CREATE PROCEDURE [Expense].[CreateExpenseItem]
	@itemDescription NVARCHAR(100),
	@transactionDate DATETIME2(0),
	@amount DECIMAL(18, 2),
	@expenseReportId INT,
	@isBillableToCustomer BIT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseItem]
		([ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer])
	VALUES (@ItemDescription,
		@TransactionDate,
		@Amount,
		@ExpenseReportId,
		@IsBillableToCustomer);
END
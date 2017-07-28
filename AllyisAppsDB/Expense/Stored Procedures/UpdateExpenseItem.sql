CREATE PROCEDURE [Expense].[UpdateExpenseItem]
	@ExpenseItemId INT,
	@ItemDescription NVARCHAR(100),
	@TransactionDate DATETIME2(0),
	@Amount DECIMAL(18, 2),
	@ExpenseReportId INT,
	@IsBillableToCustomer BIT,
	@CreatedUtc DATETIME2(0)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseItem]
		([ExpenseItemId],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[IsBillableToCustomer],
		[CreatedUtc])
	VALUES (@ExpenseItemId,
		@ItemDescription,
		@TransactionDate,
		@Amount,
		@ExpenseReportId,
		@IsBillableToCustomer,
		@CreatedUtc);
END
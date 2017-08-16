CREATE PROCEDURE [Expense].[CreateExpenseFile]
	@fileName NVARCHAR(60),
	@url NVARCHAR(250),
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseFile]
		([FileName],
		[Url],
		[ExpenseReportId])
	VALUES (@fileName,
		@url,
		@expenseReportId);
END
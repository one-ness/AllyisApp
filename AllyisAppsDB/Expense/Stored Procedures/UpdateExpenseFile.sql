CREATE PROCEDURE [Expense].[UpdateExpenseFile]
	@fileId INT,
	@fileName NVARCHAR(60),
	@url NVARCHAR(250),
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseFile]
		([FileId],
		[FileName],
		[Url],
		[ExpenseReportId])
	VALUES (@fileId,
		@fileName,
		@url,
		@expenseReportId);
END


CREATE PROCEDURE [Expense].[DeleteExpenseReport]
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS ( SELECT * FROM [Expense].[ExpenseReport] Where [ExpenseReportId] = @expenseReportId)
	BEGIN
		DELETE FROM [Expense].[ExpenseReport] WHERE [ExpenseReportId] = @expenseReportId;
		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
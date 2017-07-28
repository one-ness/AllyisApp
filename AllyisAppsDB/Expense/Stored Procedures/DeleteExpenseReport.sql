CREATE PROCEDURE [Expense].[DeleteExpenseReport]
	@ExpenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS ( SELECT * FROM [Expense].[ExpenseReport] Where [ExpenseReportId] = @ExpenseReportId)
	BEGIN
		DELETE FROM [Expense].[Expensereport] WHERE [ExpenseReportId] = @ExpenseReportId;
		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
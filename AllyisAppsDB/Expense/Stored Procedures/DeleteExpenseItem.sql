CREATE PROCEDURE [Expense].[DeleteExpenseItem]
	@expenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS ( SELECT * FROM [Expense].[ExpenseItem] Where [ExpenseItemId] = @expenseItemId)
	BEGIN
		DELETE FROM [Expense].[ExpenseItem] WHERE [ExpenseItemId] = @expenseItemId;
		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
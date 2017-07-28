CREATE PROCEDURE [Expense].[DeleteExpenseItem]
	@ExpenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS ( SELECT * FROM [Expense].[ExpenseItem] Where [ExpenseItemId] = @ExpenseItemId)
	BEGIN
		DELETE FROM [Expense].[ExpenseItem] WHERE [ExpenseItemId] = @ExpenseItemId;
		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
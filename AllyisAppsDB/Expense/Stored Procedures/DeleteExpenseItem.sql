CREATE PROCEDURE [Expense].[DeleteExpenseItem]
	@ExpenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Expense].[ExpenseItem] WHERE [ExpenseItemId] = @ExpenseItemId;
END
CREATE PROCEDURE [Expense].[UpdateExpenseHistory]
	@historyId INT,
	@text NVARCHAR(MAX)
AS

BEGIN

	UPDATE [Expense].[ExpenseReportHistory]
	SET
		[ModifiedUtc] = GETUTCDATE(),
		[Text] = @text
	WHERE [ExpenseReportHistory].[HistoryId] = @historyId;
END

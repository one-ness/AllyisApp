CREATE PROCEDURE [Expense].[GetExpenseHistory]
	@reportId INT
AS
BEGIN

	SELECT
		[HistoryId],
		[ExpenseReportId],
		[UserId],
		[Status],
		[Text],
		[CreatedUtc],
		[ModifiedUtc]
	FROM 
		[Expense].[ExpenseReportHistory] AS [ERH]
	WHERE
		[ERH].[ExpenseReportId] = @reportId
END

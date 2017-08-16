CREATE PROCEDURE [Expense].[GetReportHistory]
	@reportId INT
AS
BEGIN
	SELECT	[ExpenseReportId],
	[UserId],
	[Date],
	[Text]
	FROM [Expense].[ExpenseReportHistory] AS [ERH]
	WHERE [ERH].[ExpenseReportId] = @reportId
END
CREATE PROCEDURE [Expense].[GetExpenseFilesByExpenseReportId]
	@reportId INT
AS

BEGIN
	SELECT [FileId],
	[FileName],
	[Url]
	FROM [Expense].[ExpenseFile] AS [EF]
	WHERE [EF].[ExpenseReportId] = @reportId

END

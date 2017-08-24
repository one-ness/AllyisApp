CREATE PROCEDURE [Expense].[GetExpenseReportByExpenseReportId]
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseReportId],
		[ReportTitle],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE ExpenseReportId = @expenseReportId
END
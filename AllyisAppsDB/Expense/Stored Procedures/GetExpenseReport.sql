CREATE PROCEDURE [Expense].[GetExpenseReportByExpenseReportId]
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseReportId],
		[ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[CreatedUtc],
		[ModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE ExpenseReportId = @expenseReportId
END
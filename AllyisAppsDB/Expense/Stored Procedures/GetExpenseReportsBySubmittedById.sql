CREATE PROCEDURE [Expense].[GetExpenseReportsBySubmittedById]
	@submittedById INT
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
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE SubmittedById = @submittedById
END
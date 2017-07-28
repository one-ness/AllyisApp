CREATE PROCEDURE [Expense].[GetExpenseReportsBySubmittedById]
	@SubmittedById INT
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
	WHERE SubmittedById = @SubmittedById
END
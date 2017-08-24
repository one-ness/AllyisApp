﻿CREATE PROCEDURE [Expense].[GetExpenseReportsBySubmittedById]
	@submittedById INT
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
		[ExpenseReportModifiedUtc],
		[ExpenseReportSubmittedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE SubmittedById = @submittedById
END
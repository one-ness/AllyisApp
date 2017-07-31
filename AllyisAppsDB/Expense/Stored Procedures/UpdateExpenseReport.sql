CREATE PROCEDURE [Expense].[UpdateExpenseReport]
	@expenseReportId INT,
	@reportTitle NVARCHAR(100),
	@reportDate DATETIME2(0),
	@organizationId INT,
	@submittedById INT,
	@reportStatus TINYINT,
	@buisnessJustification NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseReport]
		([ExpenseReportId],
		[ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification])
	VALUES (@expenseReportId,
		@reportTitle,
		@reportDate,
		@organizationId,
		@submittedById,
		@reportStatus,
		@buisnessJustification);
END
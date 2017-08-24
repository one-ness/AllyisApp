CREATE PROCEDURE [Expense].[UpdateExpenseReport]
	@expenseReportId INT,
	@reportTitle NVARCHAR(100),
	@organizationId INT,
	@submittedById INT,
	@reportStatus TINYINT,
	@businessJustification NVARCHAR(100),
	@submittedUtc DATETIME2 (0)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Expense].[ExpenseReport]
	SET
		[ReportTitle] = @reportTitle,
		[ReportStatus] = @reportStatus,
		[BusinessJustification] = @businessJustification,
		[ExpenseReportSubmittedUtc] = @submittedUtc
	WHERE [ExpenseReportId] = @expenseReportId;
END
CREATE PROCEDURE [Expense].[UpdateExpenseReport]
	@expenseReportId INT,
	@reportTitle NVARCHAR(100),
	@reportDate DATETIME2(0),
	@organizationId INT,
	@submittedById INT,
	@reportStatus TINYINT,
	@businessJustification NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Expense].[ExpenseReport]
	SET
		[ReportTitle] = @reportTitle,
		[ReportDate] = @reportDate,
		[ReportStatus] = @reportStatus,
		[BusinessJustification] = @businessJustification
	WHERE [ExpenseReportId] = @expenseReportId;
END
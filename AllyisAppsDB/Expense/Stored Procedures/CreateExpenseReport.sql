CREATE PROCEDURE [Expense].[CreateExpenseReport]
	@reportTitle NVARCHAR(100),
	@organizationId INT,
	@submittedById INT,
	@reportStatus TINYINT,
	@businessJustification NVARCHAR(100),
	@createdUtc DATETIME2,
	@modifiedUtc DATETIME2,
	@submittedUtc DATETIME2,
	@reportId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseReport]
		([ReportTitle],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc],
		[ExpenseReportSubmittedUtc])
	VALUES (@reportTitle,
		@organizationId,
		@submittedById,
		@reportStatus,
		@businessJustification,
		@createdUtc,
		@modifiedUtc,
		@submittedUtc);

	SELECT IDENT_CURRENT('[Expense].[ExpenseReport]');
END
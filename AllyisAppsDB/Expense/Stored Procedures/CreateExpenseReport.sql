CREATE PROCEDURE [Expense].[CreateExpenseReport]
	@reportTitle NVARCHAR(100),
	@reportDate DATETIME2(0),
	@organizationId INT,
	@submittedById INT,
	@reportStatus TINYINT,
	@businessJustification NVARCHAR(100),
	@createdUtc DATETIME2,
	@modifiedUtc DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Expense].[ExpenseReport]
		([ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc])
	VALUES (@reportTitle,
		@reportDate,
		@organizationId,
		@submittedById,
		@reportStatus,
		@businessJustification,
		@createdUtc,
		@modifiedUtc);
END
CREATE PROCEDURE [Expense].[CreateExpenseReport]
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
		([ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification])
	VALUES (@reportTitle,
		@reportDate,
		@organizationId,
		@submittedById,
		@reportStatus,
		@buisnessJustification);
END
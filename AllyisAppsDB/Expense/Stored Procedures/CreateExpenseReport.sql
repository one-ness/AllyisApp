﻿CREATE PROCEDURE [Expense].[CreateExpenseReport]
	@ReportTitle NVARCHAR(100),
	@ReportDate DATETIME2(0),
	@OrganizationId INT,
	@SubmittedById INT,
	@ReportStatus TINYINT,
	@BuisnessJustification NVARCHAR(100),
	@CreatedUtc DATETIME2(0),
	@ModifiedUtc DATETIME2(0)
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
		[CreatedUtc],
		[ModifiedUtc])
	VALUES (@ReportTitle,
		@ReportDate,
		@OrganizationId,
		@SubmittedById,
		@ReportStatus,
		@BuisnessJustification,
		@CreatedUtc,
		@ModifiedUtc);
END
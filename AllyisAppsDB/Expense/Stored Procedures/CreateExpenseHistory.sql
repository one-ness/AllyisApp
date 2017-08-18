CREATE PROCEDURE [Expense].[CreateExpenseHistory]
	@historyId INT,
	@reportId INT,
	@userId INT,
	@status INT,
	@text NVARCHAR(MAX),
	@createdUtc datetime2(2),
	@modifiedUtc datetime2(2)
AS
BEGIN

	INSERT INTO [Expense].ExpenseReportHistory
		([HistoryId],
		[ExpenseReportId],
		[UserId],
		[CreatedUtc],
		[ModifiedUtc],
		[Text],
		[Status])
	VALUES (@historyId,
		@reportId,
		@userId,
		@createdUtc,
		@modifiedUtc,
		@text,
		@status);
END


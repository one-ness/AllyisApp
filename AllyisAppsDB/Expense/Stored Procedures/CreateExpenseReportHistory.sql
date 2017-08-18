CREATE PROCEDURE [Expense].[AddReportHistpory]
	@userId INT,
	@reportID INT,
	@text NVARCHAR(MAX)
AS
BEGIN
	
	INSERT INTO [Expense].[ExpenseReportHistory]
		([ExpenseReportId],
		[ExpenseReportId],
		[Text])
	VALUES (@userId,
		@reportId,
		@text);
END
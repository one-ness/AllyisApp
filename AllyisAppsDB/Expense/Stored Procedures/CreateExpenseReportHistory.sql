﻿CREATE PROCEDURE [Expense].[AddReportHistory]
	@userId INT,
	@reportID INT,
	@text NVARCHAR(MAX)
AS
BEGIN
	
	INSERT INTO [Expense].[ExpenseReportHistory]
		([UserId],
		[ExpenseReportId],
		[Text])
	VALUES (@userId,
		@reportId,
		@text);
END
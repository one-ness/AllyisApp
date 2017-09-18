﻿CREATE PROCEDURE [Expense].[GetReportHistory]
	@reportId INT
AS
BEGIN
	SELECT	[ExpenseReportId],
	[UserId],
	[ERH].ModifiedUtc
	[Text]
	FROM [Expense].[ExpenseReportHistory] AS [ERH]
	WHERE [ERH].[ExpenseReportId] = @reportId
END
CREATE TABLE [Expense].[ExpenseReportHistory]
(
	[HistoryId] INT NOT NULL,
	[ExpenseReportId] INT NOT NULL,
	[UserId] INT NOT NULL,
	[Status] INT NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL,
	[CreatedUtc] datetime2(2) NOT NULL,
	[ModifiedUtc] datetime2(2) NOT NULL,
	CONSTRAINT [PK_HistoryId] PRIMARY KEY CLUSTERED ([HistoryId] ASC),
	CONSTRAINT [PK_ExpenseReportHistory_ReportId] FOREIGN KEY ([ExpenseReportId]) REFERENCES [Expense].[ExpenseReport],
	CONSTRAINT [PK_ExpenseReportHistory_UserId] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User]
);

GO
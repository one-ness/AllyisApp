CREATE TABLE [Expense].[ExpenseFile] (
    [FileId]			INT             IDENTITY (102533, 2) NOT NULL,
	[FileName]			NVARCHAR (60)   NOT NULL DEFAULT('File'),
    [ExpenseReportId]	INT             NOT NULL,
	[Url]				NVARCHAR (250)	NOT NULL
    CONSTRAINT [PK_ReportFile] PRIMARY KEY CLUSTERED ([FileId] ASC)
);

GO

--CREATE TRIGGER [Expense].trg_update_file_ModifiedUtc ON [Expense].[ExpenseFile] FOR UPDATE AS
--BEGIN
--    UPDATE [Expense].[ExpenseFile] SET [ExpenseFileModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Expense].[ExpenseFile] INNER JOIN [deleted] [d] ON [ExpenseFile].[FileId] = [d].[FileId]
--END
--GO 
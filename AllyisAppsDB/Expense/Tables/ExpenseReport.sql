CREATE TABLE [Expense].[ExpenseReport] (
    [ExpenseReportId]       INT            IDENTITY (104729, 2) NOT NULL,
    [ReportTitle]           NVARCHAR (128) NOT NULL,
    [ReportDate]            DATETIME2 (0)  NOT NULL,
    [OrganizationId]        INT            NOT NULL,
    [SubmittedById]         INT            NOT NULL,
    [ReportStatus]          TINYINT        CONSTRAINT [DF_ExpenseReport_ReportStatus] DEFAULT ((1)) NOT NULL,
    [BusinessJustification] NVARCHAR (512) NOT NULL,
    [ExpenseReportCreatedUtc]            DATETIME2 (0) CONSTRAINT [DF_ExpenseReport_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [ExpenseReportModifiedUtc]           DATETIME2 (0) CONSTRAINT [DF_ExpenseReport_ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ExpenseReport] PRIMARY KEY CLUSTERED ([ExpenseReportId] ASC),
    CONSTRAINT [FK_ExpenseReport_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_ExpenseReport_User] FOREIGN KEY ([SubmittedById]) REFERENCES [Auth].[User] ([UserId])
);

GO

CREATE TRIGGER [Expense].trg_update_report_ModifiedUtc ON [Expense].[ExpenseReport] FOR UPDATE AS
BEGIN
    UPDATE [Expense].[ExpenseReport] SET [ExpenseReportModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Expense].[ExpenseReport] INNER JOIN [deleted] [d] ON [ExpenseReport].[ExpenseReportId] = [d].[ExpenseReportId]
END
GO 

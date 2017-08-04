﻿CREATE TABLE [Expense].[ExpenseItem] (
    [ExpenseItemId]        INT             IDENTITY (102533, 2) NOT NULL,
    [ItemDescription]      NVARCHAR (512)  NOT NULL,
    [TransactionDate]      DATETIME2 (0)   NOT NULL,
    [Amount]               DECIMAL (19, 4) NOT NULL,
    [ExpenseReportId]      INT             NOT NULL,
    [AccountId]            SMALLINT        NOT NULL,
    [IsBillableToCustomer] BIT             DEFAULT ((0)) NOT NULL,
    [ExpenseItemCreatedUtc]           DATETIME2 (0)   DEFAULT (getutcdate()) NOT NULL,
    [ExpenseItemModifiedUtc]          DATETIME2 (0)   DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ReportItem] PRIMARY KEY CLUSTERED ([ExpenseItemId] ASC)
);

GO

CREATE TRIGGER [Expense].trg_update_item_ModifiedUtc ON [Expense].[ExpenseItem] FOR UPDATE AS
BEGIN
    UPDATE [Expense].[ExpenseItem] SET [ExpenseItemModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Expense].[ExpenseItem] INNER JOIN [deleted] [d] ON [ExpenseItem].[ExpenseItemId] = [d].[ExpenseItemId]
END
GO 
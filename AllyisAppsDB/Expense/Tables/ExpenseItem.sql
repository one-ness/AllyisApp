CREATE TABLE [Expense].[ExpenseItem] (
    [ExpenseItemId]        INT             IdENTITY (102533, 2) NOT NULL,
    [ItemDescription]      NVARCHAR (512)  NOT NULL,
    [TransactionDate]      DATETIME2 (0)   NOT NULL,
    [Amount]               DECIMAL (19, 4) NOT NULL,
    [ExpenseReportId]      INT             NOT NULL,
    [AccountId]            SMALLINT        NOT NULL,
    [IsBillableToCustomer] BIT             CONSTRAINT [DF_ExpenseItem_IsBillableToCustomer] DEFAULT ((0)) NOT NULL,
    [CreatedUtc]           DATETIME2 (0)   CONSTRAINT [DF_ExpenseItem_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUtc]          DATETIME2 (0)   CONSTRAINT [DF_ExpenseItem_ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ReportItem] PRIMARY KEY CLUSTERED ([ExpenseItemId] ASC)
);


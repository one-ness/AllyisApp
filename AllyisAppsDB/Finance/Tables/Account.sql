CREATE TABLE [Finance].[Account] (
    [AccountId]       SMALLINT      IDENTITY (983, 1) NOT NULL,
    [AccountName]     NVARCHAR (32) NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Account_IsActive] DEFAULT ((1)) NOT NULL,
    [AccountTypeId]   TINYINT       NOT NULL,
    [ParentAccountId] SMALLINT      NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([AccountId] ASC),
    CONSTRAINT [FK_Account_Account] FOREIGN KEY ([ParentAccountId]) REFERENCES [Finance].[Account] ([AccountId]),
    CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountTypeId]) REFERENCES [Finance].[AccountType] ([AccountTypeId])
);


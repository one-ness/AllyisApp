CREATE TABLE [Finance].[AccountType] (
    [AccountTypeId]   TINYINT       NOT NULL,
    [AccountTypeName] NVARCHAR (32) NOT NULL,
    CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED ([AccountTypeId] ASC)
);


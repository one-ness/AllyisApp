CREATE TABLE [Auth].[Permission] (
    [ProductRoleId] INT NOT NULL,
    [ActionId]      INT NOT NULL,
    [IsAllowed]     BIT CONSTRAINT [DF_Permission_IsAllowed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([ActionId] ASC, [ProductRoleId] ASC)
);


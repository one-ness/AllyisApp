CREATE TABLE [Shared].[Project] (
    [ProjectId]   INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]  INT            NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Type]        NVARCHAR (20)  DEFAULT ('Hourly') NOT NULL,
    [StartUTC]    DATETIME2 (0)  DEFAULT (getutcdate()) NULL,
    [EndUTC]      DATETIME2 (0)  DEFAULT ('9999-12-31 23:59:59') NULL,
    [IsActive]    BIT            DEFAULT ((1)) NOT NULL,
    [CreatedUTC]  DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC] DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Shared.Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Shared].[Customer] ([CustomerId])
);


GO
CREATE CLUSTERED INDEX [IX_Project_CustomerId]
    ON [Shared].[Project]([CustomerId] ASC);


GO
CREATE TRIGGER [Shared].trg_update_Project ON [Shared].[Project] FOR UPDATE AS
BEGIN
	UPDATE [Shared].[Project] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Shared].[Project] INNER JOIN [deleted] [d] ON [Project].[ProjectId] = [d].[ProjectId];
END
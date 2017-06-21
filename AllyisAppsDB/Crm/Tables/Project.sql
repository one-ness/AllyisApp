CREATE TABLE [Crm].[Project] (
    [ProjectId]    INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]   INT            NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [Type]         NVARCHAR (20)  DEFAULT ('Hourly') NOT NULL,
    [StartUTC]     DATETIME2 (0)  DEFAULT (getutcdate()) NULL,
    [EndUTC]       DATETIME2 (0)  DEFAULT ('9999-12-31 23:59:59') NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    [CreatedUTC]   DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]  DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ProjectOrgId] NVARCHAR (16)  NOT NULL,
    PRIMARY KEY NONCLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Crm.Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Project]
    ON [Crm].[Project]([CustomerId] ASC);


GO
CREATE CLUSTERED INDEX [IX_Project_CustomerId]
    ON [Crm].[Project]([CustomerId] ASC);


GO
CREATE TRIGGER [Crm].trg_update_Project ON [Crm].[Project] FOR UPDATE AS
BEGIN
	UPDATE [Crm].[Project] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Crm].[Project] INNER JOIN [deleted] [d] ON [Project].[ProjectId] = [d].[ProjectId];
END
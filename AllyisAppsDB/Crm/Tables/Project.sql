CREATE TABLE [Crm].[Project] (
    [ProjectId]    INT           IDENTITY (116827, 3) NOT NULL,
    [CustomerId]   INT           NOT NULL,
    [Name]         NVARCHAR (64) NOT NULL,
    [ProjectOrgId] NVARCHAR (16) NOT NULL,
    [Type]         NVARCHAR (20) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CreatedUtc]   DATETIME2 (0) NOT NULL,
    [StartUtc]     DATETIME2 (0) NULL,
    [EndUtc]       DATETIME2 (0) NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId])
);




GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Project]
    ON [Crm].[Project]([CustomerId] ASC);


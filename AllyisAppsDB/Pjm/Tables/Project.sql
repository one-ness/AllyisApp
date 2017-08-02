CREATE TABLE [Pjm].[Project] (
    [ProjectId]    INT           IDENTITY (116827, 3) NOT NULL,
    [CustomerId]   INT           NOT NULL,
    [Name]         NVARCHAR (64) NOT NULL,
    [ProjectOrgId] NVARCHAR (16) NOT NULL,
    [IsHourly]     BIT			 DEFAULT ((0)) NOT NULL,
    [IsActive]     BIT           DEFAULT ((1)) NOT NULL,
    [CreatedUtc]   DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [StartUtc]     DATETIME2 (0) NULL,
    [EndUtc]       DATETIME2 (0) NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Project]
    ON [Pjm].[Project]([CustomerId] ASC);


CREATE TABLE [Pjm].[Project] (
    [ProjectId]    INT           IDENTITY (116827, 3) NOT NULL,
    [CustomerId]   INT           NOT NULL,
    [ProjectName]         NVARCHAR (64) NOT NULL,
    [ProjectCode] NVARCHAR (16) NOT NULL,
    [IsHourly]     BIT			 CONSTRAINT [DF_Project_IsHourly] DEFAULT ((0)) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Project_IsActive] DEFAULT ((1)) NOT NULL,
    [ProjectCreatedUtc]   DATETIME2 (0) CONSTRAINT [DF_Project_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [StartUtc]     DATETIME2 (0) NULL,
    [EndUtc]       DATETIME2 (0) NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId]),
	CONSTRAINT UC_Project_Customer UNIQUE ([CustomerId],[ProjectCode])
);


GO


CREATE NONCLUSTERED INDEX [IX_Project]
    ON [Pjm].[Project]([CustomerId] ASC);


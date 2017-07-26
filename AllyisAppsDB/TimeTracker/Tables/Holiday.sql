CREATE TABLE [TimeTracker].[Holiday] (
    [HolidayId]      INT           IDENTITY (1, 1) NOT NULL,
    [HolidayName]    NVARCHAR (64) NOT NULL,
    [Date]           DATETIME2 (0) NOT NULL,
    [OrganizationId] INT           CONSTRAINT [DF__Holiday__Organiz__0E6E26BF] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Holiday] PRIMARY KEY NONCLUSTERED ([HolidayId] ASC),
    CONSTRAINT [FK_Holiday_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);




GO



GO
CREATE CLUSTERED INDEX [IX_Holiday]
    ON [TimeTracker].[Holiday]([OrganizationId] ASC, [Date] ASC);


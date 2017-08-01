CREATE TABLE [Hrm].[Holiday] (
    [HolidayId]      INT           IDENTITY (1, 1) NOT NULL,
    [HolidayName]    NVARCHAR (64) NOT NULL,
    [Date]           DATETIME2 (0) NOT NULL,
    [OrganizationId] INT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Holiday] PRIMARY KEY NONCLUSTERED ([HolidayId] ASC),
    CONSTRAINT [FK_Holiday_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);




GO



GO
CREATE CLUSTERED INDEX [IX_Holiday]
    ON [Hrm].[Holiday]([OrganizationId] ASC, [Date] ASC);


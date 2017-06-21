CREATE TABLE [TimeTracker].[Holiday] (
    [HolidayId]      INT            IDENTITY (1, 1) NOT NULL,
    [HolidayName]    NVARCHAR (255) NOT NULL,
    [Date]           DATETIME2 (0)  NOT NULL,
    [OrganizationId] INT            DEFAULT ((0)) NOT NULL,
    [CreatedUTC]     DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([HolidayId] ASC),
    CONSTRAINT [FK_Holiday_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [UNQ_HolidayName_OrganizationId_Date] UNIQUE NONCLUSTERED ([OrganizationId] ASC, [Date] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_Holiday_OrganizationId]
    ON [TimeTracker].[Holiday]([OrganizationId] ASC);


GO
CREATE TRIGGER [TimeTracker].trg_update_Holiday ON [TimeTracker].[Holiday] FOR UPDATE AS
BEGIN
UPDATE [TimeTracker].[Holiday] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [TimeTracker].[Holiday] INNER JOIN [deleted] [d] ON [Holiday].[OrganizationId] = [d].[OrganizationId] AND [Holiday].[HolidayName] = [d].[HolidayName];
END
CREATE TABLE [TimeTracker].[Holiday]
(
	[HolidayId] INT NOT NULL IDENTITY (1,1) PRIMARY KEY NONCLUSTERED,
	[HolidayName] NVARCHAR(255) NOT NULL,
	[Date] DATETIME2(0) NOT NULL,
	[OrganizationId] INT NOT NULL DEFAULT 0,
	[CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
	CONSTRAINT [UNQ_HolidayName_OrganizationId_Date] UNIQUE ([OrganizationId], [Date]),
	CONSTRAINT [FK_Holiday_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId]),
	)
	GO
	CREATE CLUSTERED INDEX [IX_Holiday_OrganizationId] ON [TimeTracker].[Holiday]([OrganizationId]);
	GO
CREATE TRIGGER [TimeTracker].trg_update_Holiday ON [TimeTracker].[Holiday] FOR UPDATE AS
BEGIN
UPDATE [TimeTracker].[Holiday] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [TimeTracker].[Holiday] INNER JOIN [deleted] [d] ON [Holiday].[OrganizationId] = [d].[OrganizationId] AND [Holiday].[HolidayName] = [d].[HolidayName];
END
GO


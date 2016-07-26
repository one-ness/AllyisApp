CREATE TABLE [TimeTracker].[Setting]
(
	[OrganizationId] INT NOT NULL PRIMARY KEY, 
    [StartOfWeek] INT NOT NULL DEFAULT 1,
    [OvertimeHours] INT NOT NULL DEFAULT 40, 
    [OvertimePeriod] VARCHAR(10) NOT NULL DEFAULT 'Week', 
    [OvertimeMultiplier] DECIMAL(9, 4) NOT NULL DEFAULT 1.5, 
    CONSTRAINT [FK_Settings_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId])
)
GO

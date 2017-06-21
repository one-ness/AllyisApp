CREATE TABLE [TimeTracker].[Setting] (
    [OrganizationId]     INT            NOT NULL,
    [StartOfWeek]        INT            DEFAULT ((1)) NOT NULL,
    [OvertimeHours]      INT            DEFAULT ((40)) NOT NULL,
    [OvertimePeriod]     VARCHAR (10)   DEFAULT ('Week') NOT NULL,
    [OvertimeMultiplier] DECIMAL (9, 4) DEFAULT ((1.5)) NOT NULL,
    [LockDateUsed]       BIT            DEFAULT ((0)) NOT NULL,
    [LockDatePeriod]     VARCHAR (10)   DEFAULT ('Weeks') NOT NULL,
    [LockDateQuantity]   INT            DEFAULT ((2)) NOT NULL,
    PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Settings_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Setting]
    ON [TimeTracker].[Setting]([OrganizationId] ASC);


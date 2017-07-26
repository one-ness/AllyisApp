CREATE TABLE [TimeTracker].[Setting] (
    [OrganizationId]     INT            NOT NULL,
    [StartOfWeek]        INT            NOT NULL,
    [OvertimeHours]      INT            NOT NULL,
    [OvertimePeriod]     VARCHAR (10)   NOT NULL,
    [OvertimeMultiplier] DECIMAL (9, 4) NOT NULL,
    [LockDateUsed]       BIT            NOT NULL,
    [LockDatePeriod]     VARCHAR (10)   NOT NULL,
    [LockDateQuantity]   INT            NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Settings_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Setting]
    ON [TimeTracker].[Setting]([OrganizationId] ASC);


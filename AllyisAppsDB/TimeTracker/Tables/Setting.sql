﻿CREATE TABLE [TimeTracker].[Setting] (
    [OrganizationId]     INT            NOT NULL,
    [StartOfWeek]        INT            CONSTRAINT [DF_Setting_StartOfWeek] DEFAULT ((1)) NOT NULL,
    [OvertimeHours]      INT            CONSTRAINT [DF_Setting_OvertimeHours] DEFAULT ((40)) NULL,
    [OvertimePeriod]     VARCHAR (10)   CONSTRAINT [DF_Setting_OvertimePeriod] DEFAULT ('week') NOT NULL,
    [IsLockDateUsed]     BIT            CONSTRAINT [DF_Setting_LockDateUsed] DEFAULT ((0)) NOT NULL,
    [LockDatePeriod]     INT			CONSTRAINT [DF_Setting_LockDatePeriod] DEFAULT (1) NOT NULL,
    [LockDateQuantity]   INT            CONSTRAINT [DF_Setting_LockDateQuantity] DEFAULT ((14)) NOT NULL,
    [PayrollProcessedDate] DATE NULL,
    [LockDate] DATE NULL,
    [PayPeriod] VARCHAR(MAX) NOT NULL DEFAULT '{"type":"Duration","duration":"14","startDate":"2017/10/16"}',
    [OtSettingRecentlyChanged] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Settings_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);






GO
CREATE NONCLUSTERED INDEX [IX_Setting]
    ON [TimeTracker].[Setting]([OrganizationId] ASC);


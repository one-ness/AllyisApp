CREATE TABLE [Auth].[User] (
    [UserId]                INT              IdENTITY (111119, 3) NOT NULL,
    [FirstName]             NVARCHAR (32)    NOT NULL,
    [LastName]              NVARCHAR (32)    NOT NULL,
    [Email]                 NVARCHAR (384)   NOT NULL,
    [PasswordHash]          NVARCHAR (MAX)   NOT NULL,
    [EmailConfirmed]        BIT              NOT NULL,
    [PhoneNumberConfirmed]  BIT              NOT NULL,
    [TwoFactorEnabled]      BIT              NOT NULL,
    [AccessFailedCount]     INT              NOT NULL,
    [LockoutEnabled]        BIT              NOT NULL,
    [CreatedUtc]            DATETIME2 (0)    NOT NULL,
    [LanguagePreference]    INT              NULL,
    [DateOfBirth]           DATETIME2 (0)    NULL,
    [Address]               NVARCHAR (64)    NULL,
    [City]                  NVARCHAR (32)    NULL,
    [State]                 INT              NULL,
    [Country]               INT              NULL,
    [PostalCode]            NVARCHAR (16)    NULL,
    [PhoneNumber]           VARCHAR (16)     NULL,
    [PhoneExtension]        VARCHAR (8)      NULL,
    [LastSubscriptionId]    INT              NULL,
    [ActiveOrganizationId]  INT              NULL,
    [LockoutEndDateUtc]     DATETIME2 (0)    NULL,
    [PasswordResetCode]     UNIQUEIdENTIFIER NULL,
    [EmailConfirmationCode] UNIQUEIdENTIFIER NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguagePreference]) REFERENCES [Lookup].[Language] ([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([ActiveOrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_User_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId]),
    CONSTRAINT [UQ_User] UNIQUE NONCLUSTERED ([Email] ASC)
);




GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [Auth].[User]([Email] ASC, [FirstName] ASC, [LastName] ASC);


CREATE TABLE [Auth].[User] (
    [UserId]                INT              IDENTITY (111119, 3) NOT NULL,
    [FirstName]             NVARCHAR (32)    NOT NULL,
    [LastName]              NVARCHAR (32)    NOT NULL,
    [Email]                 NVARCHAR (384)   NOT NULL,
    [PasswordHash]          NVARCHAR (MAX)   NOT NULL,
    [EmailConfirmed]        BIT              DEFAULT ((0)) NOT NULL,
    [PhoneNumberConfirmed]  BIT              DEFAULT ((0)) NOT NULL,
    [TwoFactorEnabled]      BIT              DEFAULT ((0)) NOT NULL,
    [AccessFailedCount]     INT              DEFAULT ((0)) NOT NULL,
    [LockoutEnabled]        BIT              DEFAULT ((0)) NOT NULL,
    [CreatedUtc]            DATETIME2 (0)    DEFAULT (getutcdate()) NOT NULL,
    [LanguagePreference]    INT              NULL,
    [DateOfBirth]           DATETIME2 (0)    NULL,
    [AddressId]             INT              NULL,
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
    [PasswordResetCode]     UNIQUEIDENTIFIER NULL,
    [EmailConfirmationCode] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ_User] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [FK_User_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguagePreference]) REFERENCES [Lookup].[Language] ([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK_User_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId]),
    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([ActiveOrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [Auth].[User]([Email] ASC, [FirstName] ASC, [LastName] ASC);


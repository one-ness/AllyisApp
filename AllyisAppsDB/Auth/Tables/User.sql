CREATE TABLE [Auth].[User] (
    [UserId]                INT              IDENTITY (111119, 3) NOT NULL,
    [FirstName]             NVARCHAR (32)    NOT NULL,
    [LastName]              NVARCHAR (32)    NOT NULL,
    [AddressId]             INT              NOT NULL,
    [Email]                 NVARCHAR (384)   NOT NULL,
    [PasswordHash]          NVARCHAR (512)   NOT NULL,
    [EmailConfirmed]        BIT              CONSTRAINT [DF__User__EmailConfi__66603565] DEFAULT ((0)) NOT NULL,
    [PhoneNumberConfirmed]  BIT              CONSTRAINT [DF__User__PhoneNumbe__6754599E] DEFAULT ((0)) NOT NULL,
    [TwoFactorEnabled]      BIT              CONSTRAINT [DF__User__TwoFactorE__68487DD7] DEFAULT ((0)) NOT NULL,
    [AccessFailedCount]     INT              CONSTRAINT [DF__User__AccessFail__693CA210] DEFAULT ((0)) NOT NULL,
    [LockoutEnabled]        BIT              CONSTRAINT [DF__User__LockoutEna__6A30C649] DEFAULT ((0)) NOT NULL,
    [CreatedUtc]            DATETIME2 (0)    CONSTRAINT [DF__User__CreatedUtc__6B24EA82] DEFAULT (getutcdate()) NOT NULL,
    [LanguagePreference]    INT              NULL,
    [DateOfBirth]           DATETIME2 (0)    NULL,
    [PhoneNumber]           VARCHAR (16)     NULL,
    [PhoneExtension]        VARCHAR (8)      NULL,
    [LastSubscriptionId]    INT              NULL,
    [ActiveOrganizationId]  INT              NULL,
    [LockoutEndDateUtc]     DATETIME2 (0)    NULL,
    [PasswordResetCode]     UNIQUEIDENTIFIER NULL,
    [EmailConfirmationCode] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ_User] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguagePreference]) REFERENCES [Lookup].[Language] ([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([ActiveOrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
);


GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [Auth].[User]([Email] ASC, [FirstName] ASC, [LastName] ASC);


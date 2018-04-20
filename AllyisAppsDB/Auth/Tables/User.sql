CREATE TABLE [Auth].[User] (
    [UserId]                 INT              IDENTITY (111119, 3) NOT NULL,
    [FirstName]              NVARCHAR (32)    NOT NULL,
    [LastName]               NVARCHAR (32)    NOT NULL,
    [AddressId]              INT              NULL,
    [Email]                  NVARCHAR (384)   NOT NULL,
    [PasswordHash]           NVARCHAR (512)   NOT NULL,
    [IsEmailConfirmed]       BIT              CONSTRAINT [DF__User__EmailConfirmed] DEFAULT ((0)) NOT NULL,
    [IsPhoneNumberConfirmed] BIT              CONSTRAINT [DF__User__PhoneNumberConfirmed] DEFAULT ((0)) NOT NULL,
    [IsTwoFactorEnabled]     BIT              CONSTRAINT [DF__User__TwoFactorEnabled] DEFAULT ((0)) NOT NULL,
    [AccessFailedCount]      INT              CONSTRAINT [DF__User__AccessFailedCount] DEFAULT ((0)) NOT NULL,
    [IsLockoutEnabled]       BIT              CONSTRAINT [DF__User__LockoutEnabled] DEFAULT ((0)) NOT NULL,
    [UserCreatedUtc]         DATETIME2 (0)    CONSTRAINT [DF__User__UserCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PreferredLanguageId]    VARCHAR (16)     NULL,
    [DateOfBirth]            DATE             NOT NULL,
    [PhoneNumber]            VARCHAR (16)     NULL,
    [PhoneExtension]         VARCHAR (8)      NULL,
    [LastUsedSubscriptionId] INT              NULL,
    [LockoutEndDateUtc]      DATETIME2 (0)    NULL,
    [PasswordResetCode]      UNIQUEIDENTIFIER NULL,
    [EmailConfirmationCode]  UNIQUEIDENTIFIER NULL,
    [LoginProviderId]        INT              CONSTRAINT [DF_User_LoginProviderId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Language] FOREIGN KEY ([PreferredLanguageId]) REFERENCES [Lookup].[Language] ([CultureName]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK_User_Subscription] FOREIGN KEY ([LastUsedSubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]),
    CONSTRAINT [UQ_User] UNIQUE NONCLUSTERED ([Email] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_User]
	ON [Auth].[User]([Email] ASC, [FirstName] ASC, [LastName] ASC);

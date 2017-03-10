CREATE TABLE [Auth].[User] (
    [UserId]               INT  NOT NULL IDENTITY(1,1),
    [FirstName]            NVARCHAR (32) NOT NULL,
    [LastName]             NVARCHAR (32) NOT NULL,
	[Email]                NVARCHAR (384) NOT NULL,
	[PasswordHash]         NVARCHAR (MAX) NOT NULL,
	[EmailConfirmed]       BIT            NOT NULL DEFAULT 0,
	[PhoneNumberConfirmed] BIT			  NOT NULL DEFAULT 0,
    [TwoFactorEnabled]     BIT            NOT NULL DEFAULT 0,
    [AccessFailedCount]    INT            NOT NULL DEFAULT 0,
    [LockoutEnabled]       BIT            NOT NULL DEFAULT 0,
	[UserName]             NVARCHAR (256) NOT NULL,
	[CreatedUtc]           DATETIME2 (0)  CONSTRAINT [DF__User__CreatedUTC__5070F446] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUtc]          DATETIME2 (0)  CONSTRAINT [DF__User__ModifiedUT__5165187F] DEFAULT (getutcdate()) NOT NULL,
    [DateOfBirth]          DATETIME2 (0)  NULL,
    [Address]              NVARCHAR (100) NULL,
    [City]                 NVARCHAR (32) NULL,
    [State]                INT            NULL,
    [Country]              INT            NULL,
    [PostalCode]              NVARCHAR (16)  NULL,
    [PhoneNumber]          VARCHAR (32)   NULL,
    [PhoneExtension]       VARCHAR (8)   NULL,
    [LastSubscriptionId]   INT            NULL,
    [ActiveOrganizationId] INT            NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [PasswordResetCode] UNIQUEIDENTIFIER NULL, 
    [EmailConfirmationCode] UNIQUEIDENTIFIER NULL, 
    [LanguagePreference] INT NULL , 
    CONSTRAINT [PK__User__1788CC4CFBFDD246] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ__User__A9D105340B029239] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([ActiveOrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_User_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_User_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId]),
    CONSTRAINT [FK_User_Subscription] FOREIGN KEY ([LastSubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]),
	CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguagePreference]) REFERENCES [Lookup].[Language] ([id])
    ON DELETE SET NULL
    ON UPDATE NO ACTION
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_EMail]
    ON [Auth].[User]([Email] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_FK_User]
	ON [Auth].[User](ActiveOrganizationId, LastSubscriptionId, LanguagePreference, Country, State);
GO

CREATE TRIGGER [Auth].trg_update_User ON [Auth].[User] FOR UPDATE AS
BEGIN
	UPDATE [Auth].[User] SET [ModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[User] INNER JOIN [deleted] [d] ON [User].[UserId] = [d].[UserId];
END
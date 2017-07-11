﻿CREATE TABLE [Crm].[Customer] (
    [CustomerId]         INT            IDENTITY (115421, 3) NOT NULL,
    [Name]               NVARCHAR (50)  NOT NULL,
    [ContactEmail]       NVARCHAR (384) NULL,
    [Address]            NVARCHAR (100) NULL,
    [City]               NVARCHAR (100) NULL,
    [State]              INT            NULL,
    [Country]            INT            NULL,
    [PostalCode]         NVARCHAR (50)  NULL,
    [ContactPhoneNumber] VARCHAR (50)   NULL,
    [FaxNumber]          VARCHAR (50)   NULL,
    [Website]            NVARCHAR (50)  NULL,
    [EIN]                NVARCHAR (50)  NULL,
    [CreatedUTC]         DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [OrganizationId]     INT            NOT NULL,
    [IsActive]           BIT            DEFAULT ((1)) NOT NULL,
    [ModifiedUTC]        DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [CustomerOrgId]      NVARCHAR (16)  NOT NULL,
    PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Customer_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_Customer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Customer_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Customer]
    ON [Crm].[Customer]([OrganizationId] ASC, [Country] ASC, [State] ASC);


GO
CREATE TRIGGER [Crm].trg_update_Customer ON [Crm].[Customer] FOR UPDATE AS
BEGIN
	UPDATE [Crm].[Customer] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Crm].[Customer] INNER JOIN [deleted] [d] ON [Customer].[CustomerId] = [d].[CustomerId];
END
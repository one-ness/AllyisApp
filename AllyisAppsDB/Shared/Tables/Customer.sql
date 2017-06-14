CREATE TABLE [Shared].[Customer] (
    [CustomerId]         INT            IDENTITY (1, 1) NOT NULL,
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
    PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Customer_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_Customer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Customer_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId])
);


GO
CREATE TRIGGER [Shared].trg_update_Customer ON [Shared].[Customer] FOR UPDATE AS
BEGIN
    UPDATE [Shared].[Customer] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Shared].[Customer] INNER JOIN [deleted] [d] ON [Customer].[CustomerId] = [d].[CustomerId];
END
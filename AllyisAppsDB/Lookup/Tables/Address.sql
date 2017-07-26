CREATE TABLE [Lookup].[Address] (
    [AddressId]  INT           NOT NULL,
    [Address1]   NVARCHAR (64) NOT NULL,
    [Address2]   NVARCHAR (64) NULL,
    [City]       NVARCHAR (32) NOT NULL,
    [State]      NVARCHAR (32) NOT NULL,
    [PostalCode] NVARCHAR (16) NOT NULL,
    [CountryId]  INT           NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [Lookup].[Country] ([CountryId])
);


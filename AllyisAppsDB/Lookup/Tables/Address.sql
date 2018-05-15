CREATE TABLE [Lookup].[Address] (
    [AddressId]   INT           IDENTITY (198321, 3) NOT NULL,
    [Address1]    NVARCHAR (64) NULL,
    [Address2]    NVARCHAR (64) NULL,
    [City]        NVARCHAR (32) NULL,
    [StateId]     INT           NULL,
    [PostalCode]  NVARCHAR (16) NULL,
    [CountryCode] VARCHAR (8)   NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryCode]) REFERENCES [Lookup].[Country] ([CountryCode]),
    CONSTRAINT [FK_Address_State] FOREIGN KEY ([StateId]) REFERENCES [Lookup].[State] ([StateId])
);






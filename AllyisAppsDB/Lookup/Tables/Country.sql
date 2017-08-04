CREATE TABLE [Lookup].[Country] (
    [CountryId]   INT           IDENTITY (1, 1) NOT NULL,
    [CountryCode] VARCHAR (8)   NOT NULL,
    [CountryName]        NVARCHAR (96) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC),
    CONSTRAINT [UQ_Country] UNIQUE NONCLUSTERED ([CountryCode] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_Country_Code]
    ON [Lookup].[Country]([CountryCode] ASC);




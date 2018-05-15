CREATE TABLE [Lookup].[Country] (
    [CountryCode] VARCHAR (8)   NOT NULL,
    [CountryName] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Country_1] PRIMARY KEY CLUSTERED ([CountryCode] ASC),
    CONSTRAINT [UQ_Country] UNIQUE NONCLUSTERED ([CountryCode] ASC)
);










GO
CREATE NONCLUSTERED INDEX [IX_Country_Code]
    ON [Lookup].[Country]([CountryCode] ASC);




CREATE TABLE [Lookup].[Country] (
    [CountryId] INT            IDENTITY (1, 1) NOT NULL,
    [Code]      CHAR (2)       NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryId] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Country_Code]
    ON [Lookup].[Country]([Code] ASC);


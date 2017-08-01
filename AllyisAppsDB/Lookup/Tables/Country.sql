CREATE TABLE [Lookup].[Country] (
    [CountryId] INT           IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (8)   NOT NULL,
    [Name]      NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC),
    CONSTRAINT [UQ_Country] UNIQUE NONCLUSTERED ([Code] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_Country_Code]
    ON [Lookup].[Country]([Code] ASC);


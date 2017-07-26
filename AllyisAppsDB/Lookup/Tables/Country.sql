CREATE TABLE [Lookup].[Country] (
    [CountryId] INT            IdENTITY (1, 1) NOT NULL,
    [Code]      NVARCHAR (4)   NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC),
    CONSTRAINT [UQ_Country] UNIQUE NONCLUSTERED ([Code] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Country_Code]
    ON [Lookup].[Country]([Code] ASC);


CREATE TABLE [Lookup].[State] (
    [StateId]   INT           IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (8)   NOT NULL,
    [Name]      NVARCHAR (96) NOT NULL,
    [CountryId] INT           NULL,
    CONSTRAINT [PK_State] PRIMARY KEY NONCLUSTERED ([StateId] ASC),
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryId]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_States_Country] FOREIGN KEY ([Code]) REFERENCES [Lookup].[Country] ([Code])
);








GO
CREATE CLUSTERED INDEX [IX_State_Code]
    ON [Lookup].[State]([Code] ASC);


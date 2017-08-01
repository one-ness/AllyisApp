CREATE TABLE [Lookup].[State] (
    [StateId]     INT           IDENTITY (1, 1) NOT NULL,
    [CountryCode] VARCHAR (8)   NOT NULL,
    [Name]        NVARCHAR (96) NOT NULL,
    CONSTRAINT [PK_State] PRIMARY KEY NONCLUSTERED ([StateId] ASC),
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryCode]) REFERENCES [Lookup].[Country] ([CountryCode])
);










GO
CREATE CLUSTERED INDEX [IX_State_Code]
    ON [Lookup].[State]([CountryCode] ASC);




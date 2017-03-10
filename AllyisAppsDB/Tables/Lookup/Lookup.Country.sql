CREATE TABLE [Lookup].[Country]
(
	[CountryId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[Code] CHAR(2) NOT NULL UNIQUE,
    [Name] NVARCHAR(100) NOT NULL
)

GO

CREATE INDEX [IX_Country_Code] ON [Lookup].[Country] ([Code])

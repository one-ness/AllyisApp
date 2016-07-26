CREATE TABLE [Lookup].[Language]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [LanguageName] NVARCHAR(32) NULL, 
    [CultureName] NVARCHAR(8) NOT NULL
)

CREATE TABLE [Lookup].[Language] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [LanguageName] NVARCHAR (32) NULL,
    [CultureName]  NVARCHAR (8)  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


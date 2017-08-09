CREATE TABLE [Lookup].[Language] (
    [LanguageName] NVARCHAR (64) NOT NULL,
    [CultureName]  VARCHAR (16)  NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([CultureName] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Language]
    ON [Lookup].[Language]([CultureName] ASC);


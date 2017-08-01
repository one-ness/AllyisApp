CREATE TABLE [Lookup].[Language] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [LanguageName] NVARCHAR (64) NOT NULL,
    [CultureName]  VARCHAR (16)  NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Language]
    ON [Lookup].[Language]([CultureName] ASC);


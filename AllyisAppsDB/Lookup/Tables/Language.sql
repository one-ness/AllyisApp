CREATE TABLE [Lookup].[Language] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Lcid]         VARCHAR (8)   NULL,
    [LanguageName] NVARCHAR (32) NULL,
    [CultureName]  NVARCHAR (8)  NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
);




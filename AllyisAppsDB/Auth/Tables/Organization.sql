CREATE TABLE [Auth].[Organization] (
    [OrganizationId] INT            IdENTITY (112559, 7) NOT NULL,
    [Name]           NVARCHAR (64)  NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [CreatedUtc]     DATETIME2 (0)  NOT NULL,
    [SiteUrl]        NVARCHAR (384) NULL,
    [Address]        NVARCHAR (64)  NULL,
    [City]           NVARCHAR (32)  NULL,
    [State]          INT            NULL,
    [Country]        INT            NULL,
    [PostalCode]     NVARCHAR (16)  NULL,
    [PhoneNumber]    VARCHAR (16)   NULL,
    [FaxNumber]      VARCHAR (16)   NULL,
    [Subdomain]      NVARCHAR (32)  NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Organization_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_Organization_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId])
);




GO



GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Subdomain]
    ON [Auth].[Organization]([Subdomain] ASC) WHERE ([Subdomain] IS NOT NULL AND [IsActive]=(1));


GO
CREATE NONCLUSTERED INDEX [IX_Organization]
    ON [Auth].[Organization]([Country] ASC, [State] ASC);


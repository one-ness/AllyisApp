CREATE TABLE [Auth].[Organization] (
    [OrganizationId] INT            IDENTITY (112559, 7) NOT NULL,
    [Name]           NVARCHAR (64)  NOT NULL,
    [IsActive]       BIT            DEFAULT ((1)) NOT NULL,
    [CreatedUtc]     DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [SiteUrl]        NVARCHAR (384) NULL,
    [AddressId]      INT            NULL,
    [PhoneNumber]    VARCHAR (16)   NULL,
    [FaxNumber]      VARCHAR (16)   NULL,
    [Subdomain]      NVARCHAR (32)  NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Subdomain]
    ON [Auth].[Organization]([Subdomain] ASC) WHERE ([Subdomain] IS NOT NULL
                                                     AND [IsActive] = (1));

/*
GO
CREATE NONCLUSTERED INDEX [IX_Organization]
    ON [Auth].[Organization]([Country] ASC, [State] ASC);
	*/

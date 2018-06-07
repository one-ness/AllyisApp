CREATE TABLE [Auth].[Organization] (
    [OrganizationId]         INT            IDENTITY (112559, 7) NOT NULL,
    [OrganizationName]       NVARCHAR (64)  NOT NULL,
    [OrganizationStatus]     INT            NOT NULL,
    [OrganizationCreatedUtc] DATETIME2 (0)  CONSTRAINT [DF_Organization_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [SiteUrl]                NVARCHAR (384) NULL,
    [AddressId]              INT            NULL,
    [PhoneNumber]            VARCHAR (16)   NULL,
    [FaxNumber]              VARCHAR (16)   NULL,
    [Subdomain]              NVARCHAR (32)  NULL,
    [UserCount]              AS             ([Auth].[GetOrganizationUserCount]([OrganizationId])),
    [SubscriptionCount]      AS             ([Auth].[GetOrganizationSubscriptionCount]([OrganizationId])),
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Organization_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId])
);








GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Subdomain]
    ON [Auth].[Organization]([Subdomain] ASC);



/*
GO
CREATE NONCLUSTERED INDEX [IX_Organization]
    ON [Auth].[Organization]([Country] ASC, [State] ASC);
	*/

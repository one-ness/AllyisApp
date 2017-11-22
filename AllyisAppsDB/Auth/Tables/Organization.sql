CREATE TABLE [Auth].[Organization] (
    [OrganizationId]	INT            IDENTITY (112559, 7) NOT NULL,
    [OrganizationName]  NVARCHAR (64)  NOT NULL,
    [IsActive]			BIT            CONSTRAINT [DF_Organization_IsActive] DEFAULT ((1)) NOT NULL,
    [OrganizationCreatedUtc]		DATETIME2 (0)  CONSTRAINT [DF_Organization_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [SiteUrl]			NVARCHAR (384) NULL,
    [AddressId]			INT            NULL,
    [PhoneNumber]		VARCHAR (16)   NULL,
    [FaxNumber]			VARCHAR (16)   NULL,
    [Subdomain]			NVARCHAR (32)  NULL,
    [UserCount]			AS ([Auth].[GetUserCount]([OrganizationId])), 
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

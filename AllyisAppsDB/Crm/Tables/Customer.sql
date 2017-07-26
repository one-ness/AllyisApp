CREATE TABLE [Crm].[Customer] (
    [CustomerId]         INT            IdENTITY (115421, 3) NOT NULL,
    [Name]               NVARCHAR (32)  NOT NULL,
    [OrganizationId]     INT            NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF__Customer__IsActi__797309D9] DEFAULT ((1)) NOT NULL,
    [CustomerOrgId]      NVARCHAR (16)  NOT NULL,
    [CreatedUtc]         DATETIME2 (0)  CONSTRAINT [DF__Customer__Create__787EE5A0] DEFAULT (getutcdate()) NOT NULL,
    [ContactEmail]       NVARCHAR (384) NULL,
    [Address]            NVARCHAR (64)  NULL,
    [City]               NVARCHAR (32)  NULL,
    [State]              INT            NULL,
    [Country]            INT            NULL,
    [PostalCode]         NVARCHAR (16)  NULL,
    [ContactPhoneNumber] VARCHAR (16)   NULL,
    [FaxNumber]          VARCHAR (16)   NULL,
    [Website]            NVARCHAR (128) NULL,
    [EIN]                NVARCHAR (16)  NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Customer_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country] ([CountryId]),
    CONSTRAINT [FK_Customer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Customer_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State] ([StateId])
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_Customer]
    ON [Crm].[Customer]([OrganizationId] ASC, [Country] ASC, [State] ASC, [Name] ASC);


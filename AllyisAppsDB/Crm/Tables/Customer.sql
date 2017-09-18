CREATE TABLE [Crm].[Customer] (
    [CustomerId]         INT            IDENTITY (115421, 3) NOT NULL,
    [CustomerName]               NVARCHAR (32)  NOT NULL,
    [OrganizationId]     INT            NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_Customer_IsActive] DEFAULT ((1)) NOT NULL,
    [CustomerOrgId]      NVARCHAR (16)  NOT NULL,
    [CustomerCreatedUtc]         DATETIME2 (0) CONSTRAINT [DF_Customer_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [ContactEmail]       NVARCHAR (384) NULL,
    [AddressId]          INT            NULL,
    [ContactPhoneNumber] VARCHAR (16)   NULL,
    [FaxNumber]          VARCHAR (16)   NULL,
    [Website]            NVARCHAR (128) NULL,
    [EIN]                NVARCHAR (16)  NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Customer_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);

/*
GO
CREATE NONCLUSTERED INDEX [IX_Customer]
    ON [Crm].[Customer]([OrganizationId] ASC, [Country] ASC, [State] ASC, [Name] ASC);
*/

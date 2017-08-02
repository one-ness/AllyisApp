CREATE TABLE [Lookup].[OrganizationLocation] (
    [OrganizationId] INT           NOT NULL,
    [AddressId]      INT           NOT NULL,
    [LocationName]   NVARCHAR (32) NULL,
    CONSTRAINT [PK_OrganizationLocation] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [AddressId] ASC),
    CONSTRAINT [FK_OrganizationLocation_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]),
    CONSTRAINT [FK_OrganizationLocation_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);


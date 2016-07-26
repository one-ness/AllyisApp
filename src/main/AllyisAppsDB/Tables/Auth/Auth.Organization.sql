CREATE TABLE [Auth].[Organization]
(
	[OrganizationId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL, 
    [SiteUrl] NVARCHAR(100) NULL, 
    [Address] NVARCHAR(100) NULL, 
    [City] NVARCHAR(100) NULL, 
    [State] INT NULL, 
    [Country] INT NULL, 
    [PostalCode] NVARCHAR(50) NULL, 
    [PhoneNumber] VARCHAR(50) NULL, 
	[FaxNumber] VARCHAR(50) NULL,
    [Subdomain] NVARCHAR(40) NULL,
	[CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
	[ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Organization_Country] FOREIGN KEY ([Country]) REFERENCES [Lookup].[Country]([CountryId]),
	CONSTRAINT [FK_Organization_State] FOREIGN KEY ([State]) REFERENCES [Lookup].[State]([StateId])
)

GO

GO
CREATE UNIQUE NONCLUSTERED INDEX inx_Subdomain_notnull
ON [Auth].[Organization](Subdomain)
WHERE Subdomain IS NOT NULL AND IsActive = 1
GO

GO
CREATE TRIGGER [Auth].trg_update_Organization ON [Auth].[Organization] FOR UPDATE AS
BEGIN
	UPDATE [Auth].[Organization] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[Organization] INNER JOIN [deleted] [d] ON [Organization].[OrganizationId] = [d].[OrganizationId];
END
GO
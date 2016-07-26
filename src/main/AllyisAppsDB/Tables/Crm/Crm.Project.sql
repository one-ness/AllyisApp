CREATE TABLE [Crm].[Project](
	[ProjectId] [int] NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY(1,1),
	[CustomerId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](20) NOT NULL DEFAULT 'Hourly',
	[StartUTC] DATETIME2(0) DEFAULT GETUTCDATE(),
	[EndUTC] DATETIME2(0) DEFAULT '9999-12-31 23:59:59',
	[IsActive] BIT DEFAULT 1 NOT NULL,
	[CreatedUTC] DATETIME2(0) DEFAULT GETUTCDATE()

 CONSTRAINT [FK_Crm.Project_Customer] FOREIGN KEY([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId]) NOT NULL,
 [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
)
GO

CREATE CLUSTERED INDEX [IX_Project_CustomerId] ON [Crm].[Project] ([CustomerId])

GO
CREATE TRIGGER [Crm].trg_update_Project ON [Crm].[Project] FOR UPDATE AS
BEGIN
	UPDATE [Crm].[Project] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Crm].[Project] INNER JOIN [deleted] [d] ON [Project].[ProjectId] = [d].[ProjectId];
END
GO
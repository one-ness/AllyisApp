CREATE TABLE [Crm].[ProjectUser]
(
    [ProjectId] INT NOT NULL,
	[UserId] INT NOT NULL,  
	[IsActive] BIT DEFAULT 1 NOT NULL,
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
	[ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_OrganizationUser_ProjectId_UserId] PRIMARY KEY NONCLUSTERED ([ProjectId], [UserId]),
    CONSTRAINT [FK_OrganizationUser_ProjectUser] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project]([ProjectId]),
	CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User]([UserId])
)
GO

CREATE CLUSTERED INDEX [IX_ProjectUser_ProjectId] ON [Crm].[ProjectUser] ([ProjectId])

GO
CREATE TRIGGER [Crm].trg_update_ProjectUser ON [Crm].[ProjectUser] FOR UPDATE AS
BEGIN
	UPDATE [Crm].[ProjectUser] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) 
	FROM [Crm].[ProjectUser] INNER JOIN [deleted] AS [d] ON [ProjectUser].[ProjectId] = [d].[ProjectId] AND [ProjectUser].[UserId] = [d].[UserId];
	END
GO
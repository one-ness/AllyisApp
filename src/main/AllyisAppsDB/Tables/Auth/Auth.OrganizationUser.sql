CREATE TABLE [Auth].[OrganizationUser]
(
    [UserId] INT NOT NULL, 
    [OrganizationId] INT NOT NULL, 
    [EmployeeId] NVARCHAR(128) NULL,
    [OrgRoleId] INT NOT NULL,
    [TTLockDate] DATETIME2(0) NULL,
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_OrganizationUser_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId]),
    CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User]([UserId]),
    CONSTRAINT [FK_OrganizationUser_OrgRole] FOREIGN KEY ([OrgRoleId]) REFERENCES [Auth].[OrgRole]([OrgRoleId]),
    CONSTRAINT [PK_Organization_User] PRIMARY KEY NONCLUSTERED ([UserId], [OrganizationId]) 
)
GO

CREATE UNIQUE INDEX [IX_OrganizationId_EmployeeId] ON [Auth].[OrganizationUser]([OrganizationId], [EmployeeId]) WHERE [EmployeeId] IS NOT NULL
GO

CREATE CLUSTERED INDEX [IX_UserId_OrganizationId] ON [Auth].[OrganizationUser]([UserId], [OrganizationId]);
GO

CREATE INDEX [IX_EmployeeId] ON [Auth].[OrganizationUser]([EmployeeId]);
GO

CREATE TRIGGER [Auth].trg_update_OrganizationUser ON [Auth].[OrganizationUser] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[OrganizationUser] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[OrganizationUser] INNER JOIN [deleted] [d] ON [OrganizationUser].[OrganizationId] = [d].[OrganizationId] AND [OrganizationUser].[UserId] = [d].[UserId];
END
GO
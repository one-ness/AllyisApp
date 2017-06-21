CREATE TABLE [Auth].[OrganizationUser] (
    [UserId]         INT           NOT NULL,
    [OrganizationId] INT           NOT NULL,
    [EmployeeId]     NVARCHAR (16) NOT NULL,
    [OrgRoleId]      INT           NOT NULL,
    [EmployeeTypeId] INT           NOT NULL,
    [TTLockDate]     DATETIME2 (0) NULL,
    [CreatedUTC]     DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_OrganizationUser] PRIMARY KEY CLUSTERED ([UserId] ASC, [OrganizationId] ASC),
    CONSTRAINT [FK_OrganizationUser_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Auth].[EmployeeType] ([EmployeeTypeId]),
    CONSTRAINT [FK_OrganizationUser_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_OrganizationUser_OrgRole] FOREIGN KEY ([OrgRoleId]) REFERENCES [Auth].[OrgRole] ([OrgRoleId]),
    CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_OrganizationUser]
    ON [Auth].[OrganizationUser]([UserId] ASC, [OrganizationId] ASC, [OrgRoleId] ASC, [EmployeeTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeId]
    ON [Auth].[OrganizationUser]([EmployeeId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationId_EmployeeId]
    ON [Auth].[OrganizationUser]([OrganizationId] ASC, [EmployeeId] ASC) WHERE ([EmployeeId] IS NOT NULL);


GO

CREATE TRIGGER [Auth].trg_update_OrganizationUser ON [Auth].[OrganizationUser] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[OrganizationUser] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[OrganizationUser] INNER JOIN [deleted] [d] ON [OrganizationUser].[OrganizationId] = [d].[OrganizationId] AND [OrganizationUser].[UserId] = [d].[UserId];
END
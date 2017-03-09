CREATE TABLE [Auth].[Invitation]
(
    [InvitationId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [OrganizationId] INT NOT NULL,
    [Email] NVARCHAR(384) NOT NULL, 
    [FirstName] NVARCHAR(40) NOT NULL, 
    [LastName] NVARCHAR(40) NOT NULL, 
    [DateOfBirth] NVARCHAR(40) NOT NULL, 
    [AccessCode] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [OrgRole] INT NOT NULL, 
    [ProjectId] INT NULL DEFAULT NULL, 
    [CreatedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedUTC] DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(), 
    [EmployeeId] NVARCHAR(16) NOT NULL, 
    CONSTRAINT [FK_Invitation_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Auth].[Invitation]([InvitationId]),
    CONSTRAINT [FK_Invitation_OrgRole] FOREIGN KEY ([OrgRole]) REFERENCES [Auth].[OrgRole]([OrgRoleId]),
    CONSTRAINT [FK_Invitation_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project]([ProjectId]),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization]([OrganizationId])
)
GO

CREATE INDEX [IX_Invitation_InvitationId] ON [Auth].[Invitation] ([InvitationId])

GO
CREATE TRIGGER [Auth].trg_update_Invitation ON [Auth].[Invitation] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[Invitation] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[Invitation] INNER JOIN [deleted] [d] ON [Invitation].[InvitationId] = [d].[InvitationId];
END
GO
CREATE TABLE [Auth].[Invitation] (
    [InvitationId]   INT            IDENTITY (113969, 7) NOT NULL,
    [OrganizationId] INT            NOT NULL,
    [Email]          NVARCHAR (384) NOT NULL,
    [FirstName]      NVARCHAR (40)  NOT NULL,
    [LastName]       NVARCHAR (40)  NOT NULL,
    [DateOfBirth]    DATE			NOT NULL,
    [AccessCode]     VARCHAR (50)   NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [OrgRoleId]      INT            NOT NULL,
    [EmployeeTypeId] INT            NOT NULL,
    [CreatedUTC]     DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [EmployeeId]     NVARCHAR (16)  NOT NULL,
    PRIMARY KEY CLUSTERED ([InvitationId] ASC),
    CONSTRAINT [FK_Invitation_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Auth].[EmployeeType] ([EmployeeTypeId]),
    CONSTRAINT [FK_Invitation_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Auth].[Invitation] ([InvitationId]),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Invitation_OrgRole] FOREIGN KEY ([OrgRoleId]) REFERENCES [Auth].[OrgRole] ([OrgRoleId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC, [OrgRoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Invitation_InvitationId]
    ON [Auth].[Invitation]([InvitationId] ASC);


GO
CREATE TRIGGER [Auth].trg_update_Invitation ON [Auth].[Invitation] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[Invitation] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[Invitation] INNER JOIN [deleted] [d] ON [Invitation].[InvitationId] = [d].[InvitationId];
END
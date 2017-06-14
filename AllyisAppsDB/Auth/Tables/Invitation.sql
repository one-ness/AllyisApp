CREATE TABLE [Auth].[Invitation] (
    [InvitationId]   INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId] INT            NOT NULL,
    [Email]          NVARCHAR (384) NOT NULL,
    [FirstName]      NVARCHAR (40)  NOT NULL,
    [LastName]       NVARCHAR (40)  NOT NULL,
    [DateOfBirth]    NVARCHAR (40)  NOT NULL,
    [AccessCode]     VARCHAR (50)   NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [OrgRole]        INT            NOT NULL,
    [EmployeeType]   INT            NOT NULL,
    [ProjectId]      INT            DEFAULT (NULL) NULL,
    [CreatedUTC]     DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedUTC]    DATETIME2 (0)  DEFAULT (getutcdate()) NOT NULL,
    [EmployeeId]     NVARCHAR (16)  NOT NULL,
    PRIMARY KEY CLUSTERED ([InvitationId] ASC),
    CONSTRAINT [FK_Invitation_EmployeeType] FOREIGN KEY ([EmployeeType]) REFERENCES [Auth].[EmployeeType] ([EmployeeTypeId]),
    CONSTRAINT [FK_Invitation_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Auth].[Invitation] ([InvitationId]),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Invitation_OrgRole] FOREIGN KEY ([OrgRole]) REFERENCES [Auth].[OrgRole] ([OrgRoleId]),
    CONSTRAINT [FK_Invitation_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Crm].[Project] ([ProjectId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC, [OrgRole] ASC, [ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Invitation_InvitationId]
    ON [Auth].[Invitation]([InvitationId] ASC);


GO
CREATE TRIGGER [Auth].trg_update_Invitation ON [Auth].[Invitation] FOR UPDATE AS
BEGIN
    UPDATE [Auth].[Invitation] SET [ModifiedUTC] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Auth].[Invitation] INNER JOIN [deleted] [d] ON [Invitation].[InvitationId] = [d].[InvitationId];
END
CREATE TABLE [Auth].[Invitation] (
    [InvitationId]   INT            IdENTITY (113969, 7) NOT NULL,
    [OrganizationId] INT            NOT NULL,
    [Email]          NVARCHAR (384) NOT NULL,
    [FirstName]      NVARCHAR (40)  NOT NULL,
    [LastName]       NVARCHAR (40)  NOT NULL,
    [DateOfBirth]    DATE           NOT NULL,
    [AccessCode]     VARCHAR (50)   NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [OrgRoleId]      INT            NOT NULL,
    [EmployeeTypeId] TINYINT        NOT NULL,
    [CreatedUtc]     DATETIME2 (0)  CONSTRAINT [DF__Invitatio__Creat__6383C8BA] DEFAULT (getutcdate()) NOT NULL,
    [EmployeeId]     NVARCHAR (16)  NOT NULL,
    CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED ([InvitationId] ASC),
    CONSTRAINT [FK_Invitation_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Auth].[EmployeeType] ([EmployeeTypeId]),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Invitation_OrgRole] FOREIGN KEY ([OrgRoleId]) REFERENCES [Auth].[OrgRole] ([OrgRoleId])
);




GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC);


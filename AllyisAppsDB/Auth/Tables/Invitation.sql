CREATE TABLE [Auth].[Invitation] (
    [InvitationId]   INT            IDENTITY (113969, 7) NOT NULL,
    [OrganizationId] INT            NOT NULL,
    [Email]          NVARCHAR (384) NOT NULL,
    [FirstName]      NVARCHAR (32)  NOT NULL,
    [LastName]       NVARCHAR (32)  NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [OrganizationRoleId]      INT   NOT NULL,
    [InvitationCreatedUtc]     DATETIME2 (0)  CONSTRAINT [DF_Invitation_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [EmployeeId]     NVARCHAR (16)  NOT NULL,
    [DecisionDateUtc] DATETIME2(0) NULL, 
    [StatusId] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED ([InvitationId] ASC),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_Invitation_OrganizationRole] FOREIGN KEY ([OrganizationRoleId]) REFERENCES [Auth].[OrganizationRole] ([OrganizationRoleId])
);




GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC);


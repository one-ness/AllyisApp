CREATE TABLE [Auth].[Invitation] (
    [InvitationId]         INT            IDENTITY (113969, 7) NOT NULL,
    [OrganizationId]       INT            NOT NULL,
	[OrganizationRoleId]   INT			  NOT NULL,
    [Email]                NVARCHAR (384) NOT NULL,
    [FirstName]            NVARCHAR (32)  NOT NULL,
    [LastName]             NVARCHAR (32)  NOT NULL,
    [InvitationCreatedUtc] DATETIME2 (0)  CONSTRAINT [DF_Invitation_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
	[EmployeeTypeId]	   INT			  NOT NULL,
    [EmployeeId]           NVARCHAR (16)  NOT NULL,
    [DecisionDateUtc]      DATETIME2 (0)  NULL,
    [InvitationStatus]     INT            CONSTRAINT [DF__Invitatio__Statu__7C4F7684] DEFAULT ((1)) NOT NULL,
    [ProductRolesJson]     NVARCHAR (512) NOT NULL,
    CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED ([InvitationId] ASC),
    CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
	CONSTRAINT [FK_EmployeeType_Id] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Hrm].[EmployeeType] ([EmployeeTypeId])
);








GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC);


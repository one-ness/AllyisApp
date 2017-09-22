CREATE TABLE [Auth].[OrganizationUser] (
    [UserId]         INT           NOT NULL,
    [OrganizationId] INT           NOT NULL,
    [EmployeeId]     NVARCHAR (16) NOT NULL,
    [OrganizationRoleId]      INT           NOT NULL,
	[MaxAmount]					DECIMAL			 CONSTRAINT [DF__User__MaxAmount] DEFAULT ((0)) NOT NULL,
    [OrganizationUserCreatedUtc]     DATETIME2 (0) CONSTRAINT [DF_OrganizationUser_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_OrganizationUser] PRIMARY KEY CLUSTERED ([UserId] ASC, [OrganizationId] ASC),
    CONSTRAINT [FK_OrganizationUser_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_OrganizationUser_OrganizationRole] FOREIGN KEY ([OrganizationRoleId]) REFERENCES [Auth].[OrganizationRole] ([OrganizationRoleId]),
    CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);






GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser]([UserId] ASC, [OrganizationRoleId] ASC);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationUser_1]
    ON [Auth].[OrganizationUser]([OrganizationId] ASC, [EmployeeId] ASC);



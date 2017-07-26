﻿CREATE TABLE [Auth].[OrganizationUser] (
    [UserId]         INT           NOT NULL,
    [OrganizationId] INT           NOT NULL,
    [EmployeeId]     NVARCHAR (16) NOT NULL,
    [OrgRoleId]      INT           NOT NULL,
    [EmployeeTypeId] TINYINT       NOT NULL,
    [CreatedUtc]     DATETIME2 (0) DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_OrganizationUser] PRIMARY KEY CLUSTERED ([UserId] ASC, [OrganizationId] ASC),
    CONSTRAINT [FK_OrganizationUser_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [Auth].[EmployeeType] ([EmployeeTypeId]),
    CONSTRAINT [FK_OrganizationUser_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_OrganizationUser_OrgRole] FOREIGN KEY ([OrgRoleId]) REFERENCES [Auth].[OrgRole] ([OrgRoleId]),
    CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId])
);






GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser]([UserId] ASC, [OrgRoleId] ASC, [EmployeeTypeId] ASC);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationUser_1]
    ON [Auth].[OrganizationUser]([OrganizationId] ASC, [EmployeeId] ASC);


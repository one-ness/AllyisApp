
GO
PRINT N'Dropping [Auth].[OrganizationUser].[IX_OrganizationUser]...';


GO
DROP INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser];


GO
PRINT N'Dropping [Auth].[OrganizationUser].[IX_OrganizationUser_1]...';


GO
DROP INDEX [IX_OrganizationUser_1]
    ON [Auth].[OrganizationUser];


GO
PRINT N'Creating [Auth].[OrganizationUser].[IX_OrganizationUser]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser]([OrganizationId] ASC, [EmployeeId] ASC);


GO
PRINT N'Update complete.';


GO


/*
The column [Auth].[Invitation].[IsActive] is being dropped, data loss could occur.

The column [Auth].[Invitation].[StatusId] is being dropped, data loss could occur.

The column [Auth].[Invitation].[ProductRolesJson] on table [Auth].[Invitation] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
Taken Care of 


IF EXISTS (select top 1 1 from [Auth].[Invitation])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
*/
/*
The column [Finance].[Account].[OrganizationId] on table [Finance].[Account] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
This is fine on Prodcution no account yet


IF EXISTS (select top 1 1 from [Finance].[Account])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
*/


/*
The column [TimeTracker].[TimeEntry].[TimeEntryStatusId] on table [TimeTracker].[TimeEntry] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
Taken Care of 


IF EXISTS (select top 1 1 from [TimeTracker].[TimeEntry])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
*/
PRINT N'Dropping unnamed constraint on [Auth].[Invitation]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [DF__Invitatio__Statu__6C6E1476];


GO
PRINT N'Dropping [Auth].[DF_Invitation_CreatedUtc]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [DF_Invitation_CreatedUtc];


GO
PRINT N'Dropping [Finance].[DF_Account_IsActive]...';


GO
ALTER TABLE [Finance].[Account] DROP CONSTRAINT [DF_Account_IsActive];


GO
PRINT N'Dropping [TimeTracker].[DF_TimeEntry_IsLockSaved]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF_TimeEntry_IsLockSaved];


GO
PRINT N'Dropping [TimeTracker].[DF_TimeEntry_PayClassId]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF_TimeEntry_PayClassId];


GO
PRINT N'Dropping [TimeTracker].[DF_TimeEntry_CreatedUtc]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF_TimeEntry_CreatedUtc];


GO
PRINT N'Dropping [TimeTracker].[DF_TimeEntry_ModifiedUtc]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF_TimeEntry_ModifiedUtc];


GO
PRINT N'Dropping [Auth].[FK_Invitation_OrganizationId]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [FK_Invitation_OrganizationId];


GO
PRINT N'Dropping [Auth].[FK_Invitation_OrganizationRole]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [FK_Invitation_OrganizationRole];


GO
PRINT N'Dropping [Finance].[FK_Account_Account]...';


GO
ALTER TABLE [Finance].[Account] DROP CONSTRAINT [FK_Account_Account];


GO
PRINT N'Dropping [Finance].[FK_Account_AccountType]...';


GO
ALTER TABLE [Finance].[Account] DROP CONSTRAINT [FK_Account_AccountType];


GO
PRINT N'Dropping [StaffingManager].[FK_Applicant_Address]...';


GO
ALTER TABLE [StaffingManager].[Applicant] DROP CONSTRAINT [FK_Applicant_Address];


GO
PRINT N'Dropping [StaffingManager].[FK_Application_Applicant]...';


GO
ALTER TABLE [StaffingManager].[Application] DROP CONSTRAINT [FK_Application_Applicant];


GO
PRINT N'Dropping [TimeTracker].[FK_TimeEntry_Project]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [FK_TimeEntry_Project];


GO
PRINT N'Dropping [TimeTracker].[FK_TimeEntry_User]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [FK_TimeEntry_User];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_OrganizationRole]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_OrganizationRole];


GO
PRINT N'Dropping [TimeTracker].[GetLockDate]...';


GO
DROP PROCEDURE [TimeTracker].[GetLockDate];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Starting rebuilding table [Auth].[Invitation]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Auth].[tmp_ms_xx_Invitation] (
    [InvitationId]         INT            IDENTITY (113969, 7) NOT NULL,
    [OrganizationId]       INT            NOT NULL,
    [OrganizationRoleId]   INT            NOT NULL,
    [Email]                NVARCHAR (384) NOT NULL,
    [FirstName]            NVARCHAR (32)  NOT NULL,
    [LastName]             NVARCHAR (32)  NOT NULL,
    [InvitationCreatedUtc] DATETIME2 (0)  CONSTRAINT [DF_Invitation_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [EmployeeId]           NVARCHAR (16)  NOT NULL,
    [DecisionDateUtc]      DATETIME2 (0)  NULL,
    [InvitationStatus]     INT            CONSTRAINT [DF__Invitatio__Statu__7C4F7684] DEFAULT ((1)) NOT NULL,
    [ProductRolesJson]     NVARCHAR (512) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Invitation1] PRIMARY KEY CLUSTERED ([InvitationId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Auth].[Invitation])
    BEGIN
        SET IDENTITY_INSERT [Auth].[tmp_ms_xx_Invitation] ON;
        INSERT INTO [Auth].[tmp_ms_xx_Invitation] ([InvitationId], [OrganizationId], [Email], [FirstName], [LastName], [OrganizationRoleId], [InvitationCreatedUtc], [EmployeeId], [DecisionDateUtc],[ProductRolesJson])
        SELECT   [InvitationId],
                 [OrganizationId],
                 [Email],
                 [FirstName],
                 [LastName],
                 [OrganizationRoleId],
                 [InvitationCreatedUtc],
                 [EmployeeId],
                 [DecisionDateUtc],
				 '{ "200000" : 0, "300000" : 0, "400000" : 0 }'
        FROM     [Auth].[Invitation]
        ORDER BY [InvitationId] ASC;
        SET IDENTITY_INSERT [Auth].[tmp_ms_xx_Invitation] OFF;

		UPDATE [Auth].[tmp_ms_xx_Invitation] SET InvitationStatus = 2 
		FROM [Auth].[tmp_ms_xx_Invitation] JOIN [Auth].Invitation ON Invitation.InvitationId = tmp_ms_xx_Invitation.InvitationId
		WHERE Invitation.IsActive = 1 AND invitation.StatusId = 1;

		

    END

DROP TABLE [Auth].[Invitation];

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_Invitation]', N'Invitation';

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_constraint_PK_Invitation1]', N'PK_Invitation', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Auth].[Invitation].[IX_Invitation]...';


GO
CREATE NONCLUSTERED INDEX [IX_Invitation]
    ON [Auth].[Invitation]([OrganizationId] ASC);


GO
PRINT N'Starting rebuilding table [Finance].[Account]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Finance].[tmp_ms_xx_Account] (
    [AccountId]       SMALLINT      IDENTITY (983, 1) NOT NULL,
    [OrganizationId]  INT           NOT NULL,
    [AccountName]     NVARCHAR (32) NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Account_IsActive] DEFAULT ((1)) NOT NULL,
    [AccountTypeId]   TINYINT       NOT NULL,
    [ParentAccountId] SMALLINT      NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Account1] PRIMARY KEY CLUSTERED ([AccountId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Finance].[Account])
    BEGIN
        SET IDENTITY_INSERT [Finance].[tmp_ms_xx_Account] ON;
        INSERT INTO [Finance].[tmp_ms_xx_Account] ([AccountId], [AccountName], [IsActive], [AccountTypeId], [ParentAccountId])
        SELECT   [AccountId],
                 [AccountName],
                 [IsActive],
                 [AccountTypeId],
                 [ParentAccountId]
        FROM     [Finance].[Account]
        ORDER BY [AccountId] ASC;
        SET IDENTITY_INSERT [Finance].[tmp_ms_xx_Account] OFF;
    END

DROP TABLE [Finance].[Account];

EXECUTE sp_rename N'[Finance].[tmp_ms_xx_Account]', N'Account';

EXECUTE sp_rename N'[Finance].[tmp_ms_xx_constraint_PK_Account1]', N'PK_Account', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [StaffingManager].[Applicant]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [StaffingManager].[tmp_ms_xx_Applicant] (
    [ApplicantId]    INT            IDENTITY (111873, 7) NOT NULL,
    [OrganizationId] INT            CONSTRAINT [DF_Applicant_Organization] DEFAULT ((0)) NOT NULL,
    [AddressId]      INT            NULL,
    [FirstName]      NVARCHAR (32)  NOT NULL,
    [LastName]       NVARCHAR (32)  NOT NULL,
    [Email]          NVARCHAR (100) NOT NULL,
    [PhoneNumber]    VARCHAR (16)   NULL,
    [Notes]          NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Applicant1] PRIMARY KEY CLUSTERED ([ApplicantId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [StaffingManager].[Applicant])
    BEGIN
        SET IDENTITY_INSERT [StaffingManager].[tmp_ms_xx_Applicant] ON;
        INSERT INTO [StaffingManager].[tmp_ms_xx_Applicant] ([ApplicantId], [AddressId], [FirstName], [LastName], [Email], [PhoneNumber], [Notes])
        SELECT   [ApplicantId],
                 [AddressId],
                 [FirstName],
                 [LastName],
                 [Email],
                 [PhoneNumber],
                 [Notes]
        FROM     [StaffingManager].[Applicant]
        ORDER BY [ApplicantId] ASC;
        SET IDENTITY_INSERT [StaffingManager].[tmp_ms_xx_Applicant] OFF;
    END

DROP TABLE [StaffingManager].[Applicant];

EXECUTE sp_rename N'[StaffingManager].[tmp_ms_xx_Applicant]', N'Applicant';

EXECUTE sp_rename N'[StaffingManager].[tmp_ms_xx_constraint_PK_Applicant1]', N'PK_Applicant', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [StaffingManager].[Applicant].[IX_Email]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Email]
    ON [StaffingManager].[Applicant]([Email] ASC);


GO
PRINT N'Creating [StaffingManager].[Applicant].[IX_AddressId]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AddressId]
    ON [StaffingManager].[Applicant]([AddressId] ASC);


GO
PRINT N'Altering [StaffingManager].[StaffingSettings]...';


GO
ALTER TABLE [StaffingManager].[StaffingSettings]
    ADD [DefaultApplicationStatusId] INT NULL;


GO
PRINT N'Altering [TimeTracker].[Setting]...';


GO
ALTER TABLE [TimeTracker].[Setting]
    ADD [PayrollProcessedDate] DATE          NULL,
        [LockDate]             DATE          NULL,
        [PayPeriod]            VARCHAR (MAX) DEFAULT '{"type":"duration","duration":"14","startDate":"2017-10-16"}' NOT NULL;


GO
PRINT N'Starting rebuilding table [TimeTracker].[TimeEntry]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [TimeTracker].[tmp_ms_xx_TimeEntry] (
    [TimeEntryId]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]               INT            NOT NULL,
    [ProjectId]            INT            NOT NULL,
    [Date]                 DATE           NOT NULL,
    [Duration]             FLOAT (53)     NOT NULL,
    [Description]          NVARCHAR (128) NULL,
    [TimeEntryStatusId]    INT            NOT NULL,
    [IsLockSaved]          BIT            CONSTRAINT [DF_TimeEntry_IsLockSaved] DEFAULT 0 NOT NULL,
    [PayClassId]           INT            CONSTRAINT [DF_TimeEntry_PayClassId] DEFAULT 1 NOT NULL,
    [TimeEntryCreatedUtc]  DATETIME2 (0)  CONSTRAINT [DF_TimeEntry_CreatedUtc] DEFAULT getutcdate() NOT NULL,
    [TimeEntryModifiedUtc] DATETIME2 (0)  CONSTRAINT [DF_TimeEntry_ModifiedUtc] DEFAULT getutcdate() NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TimeEntry1] PRIMARY KEY NONCLUSTERED ([TimeEntryId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [TimeTracker].[TimeEntry])
    BEGIN
        SET IDENTITY_INSERT [TimeTracker].[tmp_ms_xx_TimeEntry] ON;
        INSERT INTO [TimeTracker].[tmp_ms_xx_TimeEntry] ([TimeEntryId], [UserId], [ProjectId], [Date], [Duration], [Description], [IsLockSaved], [PayClassId], [TimeEntryCreatedUtc], [TimeEntryModifiedUtc], [TimeEntryStatusId])
        SELECT [TimeEntryId],
               [UserId],
               [ProjectId],
               [Date],
               [Duration],
               [Description],
               [IsLockSaved],
               [PayClassId],
               [TimeEntryCreatedUtc],
               [TimeEntryModifiedUtc],
			   0 /*Set to Pending for all records as apoval logic is only just made*/
        FROM   [TimeTracker].[TimeEntry];
        SET IDENTITY_INSERT [TimeTracker].[tmp_ms_xx_TimeEntry] OFF;
    END

DROP TABLE [TimeTracker].[TimeEntry];

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_TimeEntry]', N'TimeEntry';

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_constraint_PK_TimeEntry1]', N'PK_TimeEntry', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [TimeTracker].[TimeEntry].[IX_FK_TimeEntry]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_TimeEntry]
    ON [TimeTracker].[TimeEntry]([UserId] ASC, [ProjectId] ASC);


GO
PRINT N'Creating [Auth].[FK_Invitation_OrganizationId]...';


GO
ALTER TABLE [Auth].[Invitation] WITH NOCHECK
    ADD CONSTRAINT [FK_Invitation_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Finance].[FK_Account_Account]...';


GO
ALTER TABLE [Finance].[Account] WITH NOCHECK
    ADD CONSTRAINT [FK_Account_Account] FOREIGN KEY ([ParentAccountId]) REFERENCES [Finance].[Account] ([AccountId]);


GO
PRINT N'Creating [Finance].[FK_Account_AccountType]...';


GO
ALTER TABLE [Finance].[Account] WITH NOCHECK
    ADD CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountTypeId]) REFERENCES [Finance].[AccountType] ([AccountTypeId]);


GO
PRINT N'Creating [Finance].[FK_Billing_SubId]...';


GO
ALTER TABLE [Finance].[Account] WITH NOCHECK
    ADD CONSTRAINT [FK_Billing_SubId] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [StaffingManager].[FK_Applicant_Address]...';


GO
ALTER TABLE [StaffingManager].[Applicant] WITH NOCHECK
    ADD CONSTRAINT [FK_Applicant_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]) ON DELETE CASCADE;


GO
PRINT N'Creating [StaffingManager].[FK_Application_Applicant]...';


GO
ALTER TABLE [StaffingManager].[Application] WITH NOCHECK
    ADD CONSTRAINT [FK_Application_Applicant] FOREIGN KEY ([ApplicantId]) REFERENCES [StaffingManager].[Applicant] ([ApplicantId]);


GO
PRINT N'Creating [TimeTracker].[FK_TimeEntry_Project]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] WITH NOCHECK
    ADD CONSTRAINT [FK_TimeEntry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]);


GO
PRINT N'Creating [TimeTracker].[FK_TimeEntry_User]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] WITH NOCHECK
    ADD CONSTRAINT [FK_TimeEntry_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [StaffingManager].[FK_DefaultApplicationStatus]...';


GO
ALTER TABLE [StaffingManager].[StaffingSettings] WITH NOCHECK
    ADD CONSTRAINT [FK_DefaultApplicationStatus] FOREIGN KEY ([DefaultApplicationStatusId]) REFERENCES [StaffingManager].[ApplicationStatus] ([ApplicationStatusId]);


GO
PRINT N'Creating [TimeTracker].[trg_update_TimeEntry]...';


GO

CREATE TRIGGER [TimeTracker].trg_update_TimeEntry ON [TimeTracker].[TimeEntry] FOR UPDATE AS
BEGIN
	UPDATE [TimeTracker].[TimeEntry] SET [TimeEntryModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
GO
PRINT N'Altering [Auth].[AcceptInvitation]...';


GO
ALTER PROCEDURE [Auth].[AcceptInvitation]
	@invitationId INT,
	@callingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @organizationId INT;
	DECLARE @organizationRole INT;
	DECLARE @email NVARCHAR(384);
	DECLARE @employeeId NVARCHAR(16);
	SELECT
		@organizationId = [OrganizationId],
		@organizationRole = [OrganizationRoleId],
		@email = [Email],
		@employeeId = [EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @invitationId AND [Invitation].InvitationStatus = 1;

	IF @organizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrieve invited user
		DECLARE @userId INT;
		SET @userId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @email
		)

		IF @userId IS NOT NULL AND @userId = @callingUserId
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION

			-- Add user to organization
			IF EXISTS (
				SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
				WHERE [OrganizationUser].[UserId] = @userId AND [OrganizationUser].[OrganizationId] = @organizationId
			)
			BEGIN -- User already in organization
				UPDATE [Auth].[OrganizationUser]
				SET [OrganizationRoleId] = @organizationRole,
					[EmployeeId] = @employeeId
				WHERE [UserId] = @userId AND 
					[OrganizationId] = @organizationId;
			END
			ELSE
			BEGIN -- User not in organization
				INSERT INTO [Auth].[OrganizationUser]  (
					[UserId], 
					[OrganizationId], 
					[OrganizationRoleId], 
					[EmployeeId]
				)
				VALUES (
					@userId, 
					@organizationId,
					@organizationRole, 
					@employeeId
				);
			END

			UPDATE [Auth].[Invitation]
			SET InvitationStatus = 1, DecisionDateUtc = GETUTCDATE()
			WHERE [InvitationId] = @invitationId;
			
			SELECT @@ROWCOUNT;

			COMMIT
		END
	END
END
GO
PRINT N'Altering [Auth].[CreateInvitation]...';


GO
ALTER PROCEDURE [Auth].[CreateInvitation]
	@email NVARCHAR(384),
	@firstName NVARCHAR(40),
	@lastName NVARCHAR(40),
	@organizationId INT,
	@organizationRole INT,
	@employeeId NVARCHAR(16),
	@prodJson NVARCHAR(384)
AS

BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
		INNER JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
		WHERE [Email] = @email AND [OrganizationId] = @organizationId
	)
	BEGIN
		SELECT -1 --Indicates the user is already in the organization
	END
	ELSE
	BEGIN
		-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [EmployeeId] = @employeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [EmployeeId] = @employeeId
		)
		BEGIN
			SELECT -2 -- Indicates employee id already taken
		END
		ELSE
		BEGIN
			INSERT INTO [Auth].[Invitation] 
				([Email], 
				[FirstName], 
				[LastName], 
				[OrganizationId],
				[OrganizationRoleId],
				[EmployeeId],
				[ProductRolesJson]
				)
			VALUES 
				(@email, 
				@firstName, 
				@lastName, 
				@organizationId,
				@organizationRole,
				@employeeId,
				@prodJson
				);

			-- Return invitation id
			SELECT SCOPE_IDENTITY()
		END
	END
END
GO
PRINT N'Altering [Auth].[DeleteInvitation]...';


GO
ALTER PROCEDURE [Auth].[DeleteInvitation]
	@invitationId INT
AS
BEGIN
	BEGIN TRANSACTION
			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @invitationId
			
			SELECT @@ROWCOUNT
	COMMIT

	
END
GO
PRINT N'Altering [Auth].[DeleteOrg]...';


GO
ALTER PROCEDURE [Auth].[DeleteOrg]
	@orgId INT
AS 
BEGIN
		UPDATE [Auth].[Organization]
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;
END
GO
PRINT N'Altering [Auth].[GetAddMemberInfo]...';


GO
ALTER PROCEDURE [Auth].[GetAddMemberInfo]
	@organizationId INT
AS
	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @organizationId
	ORDER BY [EmployeeId] DESC

	SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @organizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[ProductRole].[ProductRoleName],
		[ProductRole].[ProductRoleId],
		[ProductRole].[ProductId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Subscription].[IsActive] = 1

	/*
	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
	FROM (
		[Auth].[Organization]	WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @organizationId)
		JOIN [Pjm].[Project]	WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[ProjectName]
	*/

	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @organizationId
	ORDER BY [EmployeeId] DESC
GO
PRINT N'Altering [Auth].[GetInvitation]...';


GO
ALTER PROCEDURE [Auth].[GetInvitation]
	@inviteId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[Organization].[OrganizationId], 
		[Organization].[OrganizationName],
		[EmployeeId],
		[Invitation].[InvitationStatus],
		[Invitation].[ProductRolesJson]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Auth].[Organization].OrganizationId =  [Invitation].[OrganizationId] 
	WHERE [InvitationId] = @inviteId
GO
PRINT N'Altering [Auth].[GetInvitations]...';


GO
ALTER PROCEDURE [Auth].[GetInvitations]
	@organizationId INT,
	@statusMask int
AS
begin
	SET NOCOUNT ON;
	select * from Invitation with (nolock)
	where OrganizationId = @organizationId
	and (InvitationStatus & @statusMask) > 0
end
GO
PRINT N'Altering [Auth].[GetOrgManagementInfo]...';


GO
ALTER PROCEDURE [Auth].[GetOrgManagementInfo]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl],
		[Address].[AddressId],
		[Address].[Address1] AS 'Address',
		[Address].[City], 
		[State].[StateName] AS 'StateName', 
		[Country].[CountryName] AS 'CountryName', 
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @organizationId

	SELECT [OU].[OrganizationId],
	    [OU].[UserId],
		[OU].[OrganizationRoleId],
		[O].[OrganizationName] AS [OrganizationName],
		[OU].[EmployeeId],
		[U].[Email],
		[U].[FirstName],
		[U].[LastName],
		[OU].[MaxAmount]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @organizationId
	ORDER BY [U].[LastName]

	SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Product].[AreaUrl],
		[Product].[Description],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Sku].[SkuName],
		[Subscription].[NumberOfUsers],
		[Subscription].[SubscriptionName],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @organizationId
	AND [Subscription].[IsActive] = 1
	AND [Product].IsActive = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[OrganizationId], 
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @organizationId AND [InvitationStatus] = 1

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId AND [IsActive] = 1 
END
GO
PRINT N'Altering [Auth].[GetUser]...';


GO
ALTER procedure [Auth].[GetUser]
	@userId int
as
begin
	set nocount on

	-- get user information with address
	select u.*, a.*, s.StateName as 'State', c.CountryName as 'Country' from [User] u with (nolock)
	left join [Lookup].[Address] a with (nolock) on a.AddressId = u.AddressId
	left join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	left join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where u.UserId = @userId

	-- get list of organizations and the user role in each
	select o.*, ou.*, a.*, s.StateName as 'State', c.CountryName as 'Country' from Organization o with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	left join Lookup.Address a with (nolock) on a.AddressId = o.AddressId
	left join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	left join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where ou.UserId = @userId AND o.IsActive = 1 

	-- get a list of subscriptions and the user role in each
	select s.*, su.*, sku.SkuId, sku.IconUrl, p.ProductId, p.ProductName, p.AreaUrl from Billing.Subscription s with (nolock)
	inner join Billing.SubscriptionUser su with (nolock) on su.SubscriptionId = s.SubscriptionId
	inner join Organization o with (nolock) on o.OrganizationId = s.OrganizationId
	inner join OrganizationUser ou with (nolock) on ou.OrganizationId = o.OrganizationId
	inner join Billing.Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Billing.Product p with (nolock) on p.ProductId = sku.ProductId
	where ou.UserId = @userId and su.UserId = @userId and o.IsActive = 1 and s.IsActive = 1

	-- get list of invitations for the user
	select i.*, o.OrganizationName from [User] u with (nolock)
	left join Invitation i with (nolock) on i.Email = u.Email
	inner join Organization o with (nolock) on o.OrganizationId = i.OrganizationId
	where u.UserId = @userId and i.DecisionDateUtc is null
end
GO
PRINT N'Altering [Auth].[GetUserInvitationsByEmail]...';


GO
ALTER PROCEDURE [Auth].[GetUserInvitationsByEmail]
	@email NVARCHAR(384)
	
AS
	SET NOCOUNT ON;
SELECT 
	[InvitationId], 
	[Email], 
	[FirstName], 
	[LastName], 
	[OrganizationId],  
	[EmployeeId],
	[ProductRolesJson]
FROM [Auth].[Invitation]
WITH (NOLOCK)
WHERE [Email] = @email
GO
PRINT N'Altering [Auth].[GetUserOrgsAndInvitationInfo]...';


GO
ALTER PROCEDURE [Auth].[GetUserOrgsAndInvitationInfo]
	@userId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
			[User].[FirstName],
			[User].[LastName],
			[User].[DateOfBirth],
			[User].[Email],
			[User].[PhoneNumber],
			[User].[LastUsedSubscriptionId],
			[PreferredLanguageId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [UserId] = @userId;

	SELECT [Auth].[Organization].[OrganizationId],
		   [Organization].[OrganizationName],
		   [SiteUrl],
		   [Address1] AS 'Address',
		   [Organization].[AddressId],
		   [City],
		   [Country].[CountryName] AS 'CountryName',
		   [State].[StateName] AS 'StateName',
		   [PostalCode],
		   [PhoneNumber],
		   [FaxNumber],
		   [Subdomain],
		   [Organization].[OrganizationCreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
	RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [OrganizationUser].[UserId] = @userId 
		  AND [Auth].[Organization].[IsActive] = 1
	ORDER BY [OrganizationUser].[OrganizationRoleId] DESC, [Organization].[OrganizationName]

	SELECT 
		[InvitationId], 
		[Invitation].[Email], 
		[Invitation].[FirstName], 
		[Invitation].[LastName], 
		[Invitation].[OrganizationId],
		[Organization].[OrganizationName] AS 'OrganizationName',
		[EmployeeId],
		[ProductRolesJson]
	FROM [Auth].[User] WITH (NOLOCK)
	LEFT JOIN [Auth].[Invitation] WITH (NOLOCK) ON [User].[Email] = [Invitation].[Email]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Invitation].[OrganizationId] = [Organization].[OrganizationId]
	WHERE [User].[UserId] = @userId AND [Invitation].[InvitationStatus] = 1;

	DECLARE @addressId INT
	SET @addressId = (SELECT m.AddressId
				FROM [Auth].[User] AS m
				WHERE [UserId] = @userId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].CountryCode = [Address].CountryCode
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @addressId
END
GO
PRINT N'Altering [Auth].[RejectInvitation]...';


GO
ALTER PROCEDURE [Auth].[RejectInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET    [InvitationStatus] = 2, DecisionDateUtc = GETUTCDATE()
	WHERE [InvitationId] = @invitationId;

	SELECT @@ROWCOUNT;
END
GO
PRINT N'Altering [Auth].[UpdateMember]...';


GO
ALTER PROCEDURE [Auth].[UpdateMember]
	@userId INT,
	@orgId INT,
	@employeeId NVARCHAR(100),
	@employeeRoleId INT,
	@isInvited BIT,
	@firstName NVARCHAR(100),
	@lastName NVARCHAR (100)
AS
BEGIN
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @orgId AND [EmployeeId] = @employeeId AND [UserId] != @userId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @orgId AND [EmployeeId] = @employeeId AND [InvitationId] != @userId
		)
	BEGIN
		IF @isInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [OrganizationRoleId] = @employeeRoleId
			WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			--UPDATE [Auth].[Invitation]
			--SET [OrganizationRoleId] = @employeeRoleId,
			--	[FirstName] = @firstName,
			--	[LastName] = @lastName
			--WHERE [InvitationId] = @userId AND [OrganizationId] = @orgId;
		END
		SELECT 1;
	END
	ELSE
	BEGIN
		IF @isInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [EmployeeId] = @employeeId,
				[OrganizationRoleId] = @employeeRoleId
			WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [EmployeeId] = @employeeId,
				[FirstName] = @firstName,
				[LastName] = @lastName
			WHERE [InvitationId] = @userId AND [OrganizationId] = @orgId;
		END
		SELECT 2;
	END
END
GO
PRINT N'Altering [Finance].[CreateAccount]...';


GO
ALTER PROCEDURE [Finance].[CreateAccount]
	@accountName NVARCHAR(100),
	@organizationId INT,
	@isActive BIT,
	@accountTypeId INT,
	@parentAccountId INT,
	@returnValue INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	IF EXISTS (
		SELECT * FROM [Finance].[Account] WITH (NOLOCK)
		WHERE [AccountName] = @accountName
	)
	BEGIN
		-- Account name is not unique
		SET @returnValue = -1;
	END
	ELSE
	BEGIN
		-- Create account
		INSERT INTO [Finance].[Account]
				([AccountName],
				[OrganizationId],
				[IsActive], 
				[AccountTypeId], 
				[ParentAccountId])
		VALUES (@accountName,
				@organizationId,
				@isActive,
				@accountTypeId,
				@parentAccountId);

		SET @returnValue = SCOPE_IDENTITY();
	END
END
GO
PRINT N'Altering [Finance].[DeleteAccount]...';


GO
ALTER PROCEDURE [Finance].[DeleteAccount]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [Finance].[Account]
	WHERE [AccountId] = @accountId
END
GO
PRINT N'Altering [Finance].[GetAccountByAccountId]...';


GO
ALTER PROCEDURE [Finance].[GetAccountByAccountId]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[A].[AccountId],
		[A].[AccountName],
		[A].[OrganizationId],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN [Finance].[AccountType] AS [T] WITH (NOLOCK) ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[AccountId] = @accountId
END
GO
PRINT N'Altering [Finance].[GetAccounts]...';


GO
ALTER PROCEDURE [Finance].[GetAccounts]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[A].[AccountId],
		[A].[AccountName],
		[A].[OrganizationId],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN [Finance].[AccountType] AS [T] WITH (NOLOCK) ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [OrganizationId] = @organizationId
END
GO
PRINT N'Altering [Finance].[GetAccountsByAccountTypeId]...';


GO
ALTER PROCEDURE [Finance].[GetAccountsByAccountTypeId]
	@accountTypeId INT,
	@isActive BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[A].[AccountId],
		[A].[AccountName],
		[A].[OrganizationId],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN (SELECT * FROM [Finance].[AccountType] WHERE [AccountTypeId] = @accountTypeId) AS [T]
			ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[IsActive] = @isActive
END
GO
PRINT N'Altering [Finance].[GetAccountsByParentId]...';


GO
ALTER PROCEDURE [Finance].[GetAccountsByParentId]
	@parentAccountId INT,
	@isActive BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[A].[AccountId],
		[A].[AccountName],
		[A].[OrganizationId],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN [Finance].[AccountType] AS [T] WITH (NOLOCK) ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[ParentAccountId] = @parentAccountId AND [A].[IsActive] = @isActive
END
GO
PRINT N'Altering [StaffingManager].[CreateApplicant]...';


GO
ALTER PROCEDURE [StaffingManager].[CreateApplicant]
	@email NVARCHAR (100),
	@firstName NVARCHAR (32),
	@lastName NVARCHAR (32),
	@phoneNumber NVARCHAR (16),
	@notes NVARCHAR (MAX),
	@address1 nvarchar(64),
	@address2 nvarchar(64),
	@city nvarchar(32),
	@stateId smallint,
	@postalCode nvarchar(16),
	@countryCode varchar(8),
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		EXEC [Lookup].[CreateAddress]
			@address1,
			@address2,
			@city,
			@stateId,
			@postalCode,
			@countryCode

		INSERT INTO [StaffingManager].[Applicant]
			([AddressId],
			[FirstName],
			[LastName],
			[Email],
			[PhoneNumber],
			[Notes],
			[OrganizationId])
		VALUES
			(IDENT_CURRENT('[Lookup].[Address]'),
			@firstName,
			@lastName,
			@email,
			@phoneNumber,
			@notes,
			@organizationId);

		SELECT IDENT_CURRENT('[StaffingManager].[Applicant]') AS [ApplicantId];
	COMMIT TRANSACTION
END
GO
PRINT N'Altering [StaffingManager].[GetApplicantById]...';


GO
ALTER PROCEDURE [StaffingManager].[GetApplicantById]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Applicant] WHERE [ApplicantId] = @applicantId
END
GO
PRINT N'Altering [StaffingManager].[GetStaffingDefaultStatus]...';


GO
ALTER PROCEDURE [StaffingManager].[GetStaffingDefaultStatus]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [DefaultPositionStatusId],
		   [DefaultApplicationStatusId]
	FROM [StaffingManager].[StaffingSettings]
	WHERE [StaffingSettings].[OrganizationId] = @organizationId

END
GO
PRINT N'Altering [TimeTracker].[GetSettings]...';


GO
ALTER PROCEDURE [TimeTracker].[GetSettings]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT	[OrganizationId],
			[StartOfWeek],
			[OvertimeHours],
			[OvertimePeriod],
			[OvertimeMultiplier],
			[IsLockDateUsed],
			[LockDatePeriod],
			[LockDateQuantity],
			[PayrollProcessedDate],
			[LockDate],
			[PayPeriod]
	FROM [TimeTracker].[Setting] WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId;
GO
PRINT N'Altering [TimeTracker].[GetTimeEntryIndexInfo]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntryIndexInfo]
	@organizationId INT,
	@userId INT,
	@productId INT,
	@startingDate DATE,
	@endingDate DATE
AS
	SET NOCOUNT ON;

	-- Settings is declared as a table here so that the StartOfWeek field can be used in other Select
	-- blocks lower in this same stored procedure, while also letting the settings table itself be returned
	DECLARE @settings TABLE (
		StartOfWeek INT,
		IsLockDateUsed INT,
		LockDatePeriod VARCHAR(10),
		LockDateQuantity INT
	);
	INSERT INTO @settings (StartOfWeek, IsLockDateUsed, LockDatePeriod, LockDateQuantity)
	SELECT [StartOfWeek], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId

	-- Starting and Ending date parameters are adjusted if the input is null, using the StartOfWeek from above
	DECLARE @startOfWeek INT;
	SET @startOfWeek = (
		SELECT TOP 1
			[StartOfWeek]
		FROM @settings
	)
	DECLARE @todayDayOfWeek INT;
	SET @todayDayOfWeek = ((6 + DATEPART(dw, GETDATE()) + @@dATEFIRST) % 7);

	IF(@startingDate IS NULL)
	BEGIN
		DECLARE @daysIntoWeek INT;
		IF (@todayDayOfWeek < @startOfWeek)
			SET @daysIntoWeek = @startOfWeek - @todayDayOfWeek - 7;
		ELSE
			SET @daysIntoWeek = @startOfWeek - @todayDayOfWeek;
		SET @startingDate = DATEADD(dd, @daysIntoWeek, GETDATE());
	END

	IF(@endingDate IS NULL)
	BEGIN
		DECLARE @daysLeftInWeek INT;
		IF (@todayDayOfWeek < @startOfWeek)
			SET @daysLeftInWeek = @startOfWeek - @todayDayOfWeek - 1;
		ELSE
			SET @daysLeftInWeek = @startOfWeek - @todayDayOfWeek + 6;
		SET @endingDate = DATEADD(dd, @daysLeftInWeek, GETDATE());
	END

	-- Begin select statements

	SELECT * FROM @settings

	
	SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;


	SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId ORDER BY [Date];


	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[StartUtc] as [StartDate],
			[Project].[EndUtc] as [EndDate],
			[Project].[ProjectName] AS [ProjectName],
			[Project].[IsActive],
			[Project].[IsHourly] AS [IsHourly],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[ProjectOrgId]
	FROM (
		(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
		FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @userId AND [OrganizationId] = @organizationId)
		AS [OrganizationUser]
		JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
		JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
		JOIN ( [Pjm].[Project]
			JOIN [Pjm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
		)
										ON [Project].[CustomerId] = [Customer].[CustomerId]
										AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
	)
	UNION ALL
	SELECT	[ProjectId],
			[CustomerId],
			0,
			[ProjectCreatedUtc],
			[StartUtc],
			[EndUtc],
			[ProjectName],
			[IsActive],
			[IsHourly],
			(SELECT [OrganizationName] FROM [Auth].[Organization] WITH (NOLOCK) WHERE [OrganizationId] = 0),
			(SELECT [CustomerName] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [CustomerId] = 0),
			NULL,
			0,
			0,
			[ProjectOrgId]
			FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = 0
	ORDER BY [Project].[ProjectName]

	SELECT [User].[UserId],
		[User].[FirstName],
		[User].[LastName],
		[User].[Email]
	FROM [Auth].[User] WITH (NOLOCK) 
	LEFT JOIN [Billing].[SubscriptionUser]	WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
	WHERE 
		[Subscription].[SubscriptionId] = (
		SELECT [SubscriptionId] 
		FROM [Billing].[Subscription] WITH (NOLOCK) 
		LEFT JOIN [Billing].[Sku]		WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
		LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
		WHERE [Subscription].[OrganizationId] = @organizationId
			AND [Sku].[ProductId] = @productId
			AND [Subscription].[IsActive] = 1
		)
	ORDER BY [User].[LastName]

	SELECT DISTINCT [TimeEntryId] 
		,[User].[UserId] AS [UserId]
		,[User].[FirstName] AS [FirstName]
		,[User].[LastName] AS [LastName]
		,[User].[Email]
		,[OrganizationUser].[EmployeeId]
		,[TimeEntry].[ProjectId]
		,[TimeEntry].[PayClassId]
		,[PayClass].[PayClassName] AS [PayClassName]
		,[Date]
		,[Duration]
		,[Description]
		,[TimeEntryStatusId]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
	JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
	JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @organizationId
	WHERE [User].[UserId] = @userId
		AND [Date] >= @startingDate
		AND [Date] <= @endingDate
		AND [PayClass].[OrganizationId] = @organizationId
	ORDER BY [Date] ASC
GO
PRINT N'Altering [TimeTracker].[UpdateLockDate]...';


GO
ALTER PROCEDURE [TimeTracker].[UpdateLockDate]
	@organizationId INT,
	@lockDate DATE
AS
	UPDATE [TimeTracker].[Setting]
	SET [LockDate] = @lockDate
	WHERE [OrganizationId] = @organizationId;
GO
PRINT N'Altering [TimeTracker].[CreateBulkTimeEntry]...';


GO
ALTER PROCEDURE [TimeTracker].[CreateBulkTimeEntry]
	@date DATETIME2(0),
	@duration FLOAT,
	@description NVARCHAR(120),
	@payClassId INT,
	@organizationId INT,
	@overwrite BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @tTProductId INT = (SELECT [ProductId] FROM [Billing].[Product] WHERE [Product].[ProductName] = 'TimeTracker');
	SELECT [SkuId] INTO #SKUIDs FROM [Billing].[Sku] WHERE [ProductId] = @tTProductId;

	DECLARE @subscriptionId INT = 
		(SELECT TOP 1 [SubscriptionId] 
		FROM [Billing].[Subscription] WITH (NOLOCK) 
		WHERE [OrganizationId] = @organizationId 
			AND [SkuId] IN (SELECT [SkuId] FROM #SKUIDs));
	SELECT [UserId], 
		@date AS [Date],
			@duration AS [Duration], 
			@description AS [Description], 
			1 AS [IsActive], 
			@payClassId AS [PayClassId], 
			NULL AS 'FirstProject' 
	INTO #OrgTmp 
	FROM [Billing].[SubscriptionUser] WITH (NOLOCK) WHERE [SubscriptionId] = @subscriptionId;

	Declare @firstProject as int
	set @firstProject = (SELECT TOP 1 [ProjectId] 
		FROM [Pjm].[ProjectUser], #OrgTmp WITH (NOLOCK) 
		WHERE [ProjectUser].[UserId] = [#OrgTmp].[UserId] AND [ProjectUser].[IsActive] = 1 
			AND [ProjectId] IN 
				(SELECT [ProjectId] 
				FROM [Pjm].[Project] WITH (NOLOCK) 
				JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [Customer].[OrganizationId] = @organizationId));
	--^^^ Sets the column that contains the first project id for each user for the specified org

	if (@firstProject is null)
	begin
		set @firstProject = 0
	end

	UPDATE #OrgTmp SET [FirstProject] = @firstProject
				
	IF (@overwrite = 1)
	BEGIN
		UPDATE [TimeTracker].[TimeEntry]
		SET	[Duration] = @duration,
			[Description] = @description,
			[PayClassId] = @payClassId
		WHERE [Date] = @date
		AND [UserId] IN (SELECT [UserId] FROM #OrgTmp)
	END

	DELETE FROM #OrgTmp WHERE [UserId] IN (SELECT [UserId] FROM [TimeTracker].[TimeEntry] WHERE [Date] = @date);

	INSERT INTO [TimeTracker].[TimeEntry] ([UserId], [Date], [Duration], [Description], [PayClassId], [ProjectId])SELECT [UserId], [Date], [Duration], [Description], [PayClassId], [FirstProject] AS 'ProjectId' FROM #OrgTmp;
	DROP TABLE #OrgTmp;
	DROP TABLE #SKUIDs;
END
GO
PRINT N'Altering [TimeTracker].[CreateTimeEntry]...';


GO
ALTER PROCEDURE [TimeTracker].[CreateTimeEntry]
	@userId INT,
	@projectId INT,
	@payClassId INT,
	@date DATE,
	@duration FLOAT,
	@description NVARCHAR(120),
	@timeEntryStatusId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON
	INSERT INTO [TimeTracker].[TimeEntry]
			   ([UserId]
			   ,[ProjectId]
			   ,[PayClassId]
			   ,[Date]
			   ,[Duration]
			   ,[Description]
			   ,[TimeEntryStatusId])
		 VALUES(@userId
			   ,@projectId
			   ,@payClassId
			   ,@date
			   ,@duration
			   ,@description
			   ,@timeEntryStatusId);
	SELECT SCOPE_IDENTITY();
END
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesByUserOverDateRange]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
    @userId [Auth].[UserTable] READONLY,
    @organizationId INT,
    @startingDate DATE,
    @endingDate DATE
AS
SET NOCOUNT ON;
SELECT DISTINCT
          [te].[TimeEntryId]
         ,[te].[Date]
         ,[te].[Duration]
         ,[te].[Description]
         ,[te].[TimeEntryStatusId]
         ,[te].[ProjectId]
         ,[te].[PayClassId]
         ,[u].[UserId] AS [UserId]
         ,[u].[FirstName] AS [FirstName]
         ,[u].[LastName] AS [LastName]
         ,[u].[Email]
         ,[ou].[EmployeeId]
         ,[pc].[PayClassName] AS [PayClassName]
         ,[IsLocked] = CAST(
             CASE
                 WHEN ([s].[LockDate] IS NOT NULL
                         AND [te].[Date] <= [s].[LockDate])
                     OR ([s].[LockDate] IS NULL
                         AND [te].[Date] <= [s].[PayrollProcessedDate])
                 THEN 1
                 ELSE 0
             END
         AS BIT)
    FROM [TimeTracker].[TimeEntry] [te] WITH (NOLOCK) 
    JOIN [Auth].[User]              [u] WITH (NOLOCK) ON [u].[UserId] = [te].[UserId]
    JOIN [Hrm].[PayClass]          [pc] WITH (NOLOCK) ON [pc].[PayClassId] = [te].[PayClassId]
    JOIN [Auth].[OrganizationUser] [ou] WITH (NOLOCK) ON [u].[UserId] = [ou].[UserId] AND [ou].[OrganizationId] = @organizationId
    JOIN [TimeTracker].[Setting]    [s] WITH (NOLOCK) ON [s].[OrganizationId] = [ou].[OrganizationId]
   WHERE [u].[UserId] IN (SELECT [userId] FROM @userId)
     AND [te].[Date] >= @startingDate
     AND [te].[Date] <= @endingDate
     AND [pc].[OrganizationId] = @organizationId
ORDER BY [te].[Date] ASC
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesOverDateRange]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesOverDateRange]
    @organizationId INT,
    @startingDate DATE,
    @endingDate DATE
AS
SET NOCOUNT ON;
SELECT DISTINCT 
          [te].[TimeEntryId]
         ,[te].[ProjectId]
         ,[te].[PayClassId]
         ,[te].[Date]
         ,[te].[Duration]
         ,[te].[Description]
         ,[te].[TimeEntryStatusId]
         ,[u].[UserId] AS [UserId]
         ,[u].[FirstName] AS [FirstName]
         ,[u].[LastName] AS [LastName]
         ,[u].[Email]
         ,[ou].[EmployeeId]
         ,[pc].[PayClassName] AS [PayClassName]
         ,[IsLocked] = CAST(
             CASE
                 WHEN ([s].[LockDate] IS NOT NULL
                         AND [te].[Date] <= [s].[LockDate])
                     OR ([s].[LockDate] IS NULL
                         AND [te].[Date] <= [s].[PayrollProcessedDate])
                 THEN 1
                 ELSE 0
             END
         AS BIT)
    FROM [TimeTracker].[TimeEntry] [te] WITH (NOLOCK)
    JOIN [Hrm].[PayClass]          [pc] WITH (NOLOCK) ON [pc].[PayClassId] = [te].[PayClassId]
    JOIN [Auth].[User]              [u] WITH (NOLOCK) ON [u].[UserId] = [te].[UserId]
    JOIN [Auth].[OrganizationUser] [ou] WITH (NOLOCK) ON [u].[UserId] = [ou].[UserId] AND [ou].[OrganizationId] = @organizationId
    JOIN [TimeTracker].[Setting]    [s] WITH (NOLOCK) ON [s].[OrganizationId] = [ou].[OrganizationId]
   WHERE [te].[Date] >= @startingDate
     AND [te].[Date] <= @endingDate
     AND [pc].[OrganizationId] = @organizationId
ORDER BY [te].[Date] ASC
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesThatUseAPayClass]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesThatUseAPayClass]
	@payClassId INT
AS
	SET NOCOUNT ON;
SELECT DISTINCT
	[TimeEntryId],
	[ProjectId],
	[PayClassId],
	[Duration],
	[Description],
	[IsLockSaved],
	[TimeEntryStatusId]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
WHERE [PayClassId] = @payClassId
GO
PRINT N'Altering [TimeTracker].[GetTimeEntryById]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntryById]
	@timeEntryId INT
AS
	SET NOCOUNT ON;
	SELECT [UserId],
		[ProjectId],
		[PayClassId],
		[Date],
		[Duration],
		[Description],
		[IsLockSaved],
		[TimeEntryStatusId]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	WHERE [TimeEntryId] = @timeEntryId
GO
PRINT N'Altering [TimeTracker].[UpdateTimeEntry]...';


GO
ALTER PROCEDURE [TimeTracker].[UpdateTimeEntry]
    @timeEntryId INT,
    @projectId INT,
    @payClassId INT,
    @duration FLOAT,
    @description NVARCHAR(120),
    @isLockSaved BIT,
    @timeEntryStatusId INT
AS
    SET NOCOUNT ON;
UPDATE [TimeTracker].[TimeEntry]
   SET [ProjectId] = @projectId
      ,[PayClassId] = @payClassId
      ,[Duration] = @duration
      ,[Description] = @description
      ,[IsLockSaved] = @isLockSaved
      ,[TimeEntryStatusId] = @timeEntryStatusId
 WHERE [TimeEntryId] = @timeEntryId
GO
PRINT N'Altering [Hrm].[CreateHoliday]...';


GO
ALTER PROCEDURE [Hrm].[CreateHoliday]
	@holidayName NVARCHAR(50),
	@date DATE,
	@organizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[Holiday] ([HolidayName], [Date], [OrganizationId]) VALUES (@holidayName, @date, @organizationId);
	
	declare @payClassIdForHoliday int;
		
	IF (SELECT COUNT([PayClassId]) FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday') <= 0
	BEGIN
		EXEC [Hrm].[CreatePayClass] @payClassName = 'Holiday', @organizationId = @organizationId
	END

	SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');

	EXEC [TimeTracker].[CreateBulkTimeEntry]
		@date = @date,
		@duration = 8, 
		@description = @holidayName,
		@payClassId = @payClassIdForHoliday,
		@organizationId = @organizationId,
		@overwrite = 0;
SELECT SCOPE_IDENTITY();
GO
PRINT N'Altering [Billing].[CreateSubscription]...';


GO
ALTER PROCEDURE [Billing].[CreateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50),
	@userId INT,
	@productRoleId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @subscriptionId INT

	--Create the new subscription
	INSERT INTO [Billing].[Subscription]
		([OrganizationId],
		[SkuId],
		[SubscriptionName])
	VALUES
		(@organizationId,
		@skuId,
		@subscriptionName);

	select @subscriptionId = SCOPE_IDENTITY()
	
	-- create a new subscription user
	INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
		values(@userId, @subscriptionId, @productRoleId)

	select @subscriptionId

END
GO
PRINT N'Altering [Billing].[GetProductList]...';


GO
ALTER PROCEDURE [Billing].[GetProductList]
AS
	SET NOCOUNT ON;
	SELECT * from Product with (nolock)
	order by ProductName asc
GO
PRINT N'Altering [Billing].[GetSubscriptionDetailsById]...';


GO
ALTER PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
begin
	SET NOCOUNT ON;
	select s.*, sku.SkuName, sku.[Description] as 'SkuDescription', sku.ProductId, sku.IconUrl, p.AreaUrl, p.[Description] as 'ProductDescription', p.ProductName from Subscription s with (nolock)
	inner join Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Product p with (nolock) on p.ProductId = sku.ProductId
	where s.SubscriptionId = @subscriptionId and s.IsActive = 1 and sku.IsActive = 1 and p.IsActive = 1
end
GO
PRINT N'Altering [Lookup].[CreateTag]...';


GO
ALTER PROCEDURE [Lookup].[CreateTag]
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Lookup].[Tag] 
		([TagName])
	VALUES 	 
		(@tagName)

	SELECT IDENT_CURRENT('[Lookup].[TagId]');
END
GO
PRINT N'Altering [Lookup].[SetupTag]...';


GO
ALTER PROCEDURE [Lookup].[SetupTag]
	@positionId INT,
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		EXEC [Lookup].[CreateTag] @tagName;

		DECLARE @tagId INT 
		SET @tagId = IDENT_CURRENT('[Lookup].[TagId]');

		EXEC [StaffingManager].[CreatePositionTag] @tagId, @positionId;

		SELECT IDENT_CURRENT('[Lookup].[TagId]');
	COMMIT TRANSACTION
END
GO
PRINT N'Altering [Lookup].[GetCountries]...';


GO
ALTER PROCEDURE [Lookup].[GetCountries]
AS
	SET NOCOUNT ON;
	SELECT * FROM [Lookup].[Country] WITH (NOLOCK)
	order by CountryName asc
GO
PRINT N'Altering [Lookup].[GetLanguages]...';


GO
ALTER PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Language] WITH (NOLOCK)
END
GO
PRINT N'Altering [Lookup].[GetStates]...';


GO
ALTER PROCEDURE [Lookup].[GetStates]
	@countryCode varchar(8)
AS
BEGIN
	SET NOCOUNT ON;
	select * from [State] with (nolock) where [State].CountryCode = @countryCode
	order by CountryCode, StateName asc
END
GO
PRINT N'Altering [Lookup].[GetStatesByCountry]...';


GO
ALTER PROCEDURE [Lookup].[GetStatesByCountry]
	@countryName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF ((SELECT Count(*) FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @countryName) <> 0)

			SELECT [State].[StateName]
			FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @countryName
			ORDER BY [State].[StateName] asc
		
	ELSE
		SELECT @countryName
			 
END
GO
PRINT N'Altering [StaffingManager].[GetPosition]...';


GO
ALTER PROCEDURE [StaffingManager].[GetPosition]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Position].[PositionId],
		[Position].[OrganizationId],
		[CustomerId],
		[Position].[AddressId],  
		[StartDate], 
		[Position].[PositionStatusId],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[Position].[EmploymentTypeId],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[Position].[PositionLevelId],
		[HiringManager],
		[TeamName],
		[Address].[Address1],
		[Address].[City],
		[State].[StateName],
		[Country].[CountryName],
		[Address].[PostalCode],
		[PositionCreatedUtc],
		[PositionModifiedUtc],
		[EmploymentType].[EmploymentTypeName],
		[PositionStatus].[PositionStatusName],
		[PositionLevel].[PositionLevelName]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]		WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]		WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]			WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	LEFT JOIN [StaffingManager].[PositionTag]	WITH (NOLOCK) ON [PositionTag].[PositionId] = [Position].[PositionId]
	LEFT JOIN [StaffingManager].[EmploymentType] WITH (NOLOCK) ON [EmploymentType].[EmploymentTypeId] = [Position].[EmploymentTypeId]
	LEFT JOIN [StaffingManager].[PositionStatus] WITH (NOLOCK) ON [PositionStatus].[PositionStatusId] = [Position].[PositionStatusId]
	LEFT JOIN [StaffingManager].[PositionLevel] WITH (NOLOCK) ON [PositionLevel].[PositionLevelId] = [Position].[PositionLevelId]
	WHERE [Position].[PositionId] = @positionId

	-- Select all tags from the position
	SELECT
		[Tag].[TagId],
		[Tag].[TagName]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[PositionId] = @positionId
END
GO
PRINT N'Altering [StaffingManager].[GetPositionsByorganizationId]...';


GO
ALTER PROCEDURE [StaffingManager].[GetPositionsByorganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionId],
		[OrganizationId],
		[CustomerId],
		[Position].[AddressId],
		[StartDate], 
		[PositionStatusId],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[EmploymentTypeId],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[PositionLevelId],
		[HiringManager],
		[TeamName],
		[Address].[Address1],
		[Address].[Address2],
		[Address].[City],
		[State].[StateName],
		[Country].[CountryName],
		[Address].[PostalCode]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC

	-- Select all tags from the position
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
END
GO
PRINT N'Altering [StaffingManager].[GetStaffingIndexInfo]...';


GO
ALTER PROCEDURE [StaffingManager].[GetStaffingIndexInfo]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Position].[PositionId],
		[Position].[OrganizationId],
		[Position].[CustomerId],
		[Position].[AddressId],
		[PositionCreatedUtc],
		[PositionModifiedUtc],
		[StartDate], 
		[PositionStatusId],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[EmploymentTypeId],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[PositionLevelId],
		[HiringManager],
		[TeamName],
		[Address].[Address1],
		[Address].[Address2],
		[Address].[City],
		[State].[StateName],
		[Country].[CountryName],
		[Address].[PostalCode]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]				WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]				WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]					WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[Position].[StartDate] ASC

	-- Select all tags from the positions
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
	
	-- Select all Employment Types from the org
		SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [StaffingManager].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId

	-- Select all Position Levels from the org
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [StaffingManager].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId

	-- Select all Position Status' from the org
		SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [StaffingManager].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
	
	-- Select all application Status' from the org
		SELECT [ApplicationStatusId],
		[OrganizationId],
		[ApplicationStatusName]
	FROM [StaffingManager].[ApplicationStatus]
	WHERE [ApplicationStatus].[OrganizationId] = @organizationId

	-- Select all Customers for each position
	SELECT 
		[Customer].[CustomerId],
		[Customer].[CustomerName],
		[Customer].[AddressId],
		[Customer].[ContactEmail],
		[Customer].[ContactPhoneNumber],
		[Customer].[FaxNumber],
		[Customer].[Website],
		[Customer].[EIN],
		[Customer].[CustomerCreatedUtc],
		[Customer].[OrganizationId],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive]
    FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK)
    WHERE [Customer].[OrganizationId] = @organizationId
END
GO
PRINT N'Altering [StaffingManager].[GetStaffingIndexInfoFiltered]...';


GO
ALTER PROCEDURE [StaffingManager].[GetStaffingIndexInfoFiltered]
	@organizationId INT,
	@status			[StaffingManager].[StatusesTable] READONLY,
	@type			[StaffingManager].[TypesTable] READONLY,
	@tags			[Lookup].[TagTable] READONLY
AS
BEGIN
	DECLARE @sSQL NVARCHAR(MAX), @Where NVARCHAR(MAX) = ''
	DECLARE @order NVARCHAR(100) = ' ORDER BY [StaffingManager].[Position].[StartDate] ASC'
	SET @sSQL =
		'SELECT DISTINCT [Position].[PositionId],
			[Position].[OrganizationId],
			[Position].[CustomerId],
			[Position].[AddressId],
			[StartDate], 
			[Position].[PositionStatusId],
			[PositionTitle], 
			[BillingRateFrequency],
			[BillingRateAmount],
			[DurationMonths],
			[Position].[EmploymentTypeId],
			[PositionCount],
			[RequiredSkills],
			[JobResponsibilities],
			[DesiredSkills],
			[PositionLevelId],
			[HiringManager],
			[TeamName],
			[Address].[Address1],
			[Address].[Address2],
			[Address].[City],
			[State].[StateName],
			[Country].[CountryName],
			[Address].[PostalCode],
			[Customer].[CustomerId],
			[Customer].[CustomerName],
			[Customer].[AddressId],
			[Customer].[ContactEmail],
			[Customer].[ContactPhoneNumber],
			[Customer].[FaxNumber],
			[Customer].[Website],
			[Customer].[EIN],
			[Customer].[CustomerCreatedUtc],
			[Customer].[OrganizationId],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive]
		FROM [StaffingManager].[Position]
		LEFT JOIN [StaffingManager].[PositionTag]	 WITH (NOLOCK) ON [PositionTag].[PositionId] = [Position].[PositionId]
			 JOIN [Lookup].[Tag]					 WITH (NOLOCK) ON [PositionTag].[TagId] = [Tag].[TagId]
		LEFT JOIN [Lookup].[Address]				 WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
		LEFT JOIN [Lookup].[Country]				 WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]					 WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
		LEFT JOIN [CRM].[Customer]					 WITH (NOLOCK) ON [Customer].[CustomerId] = [Position].[CustomerId]
		LEFT JOIN [StaffingManager].[EmploymentType] WITH (NOLOCK) ON [EmploymentType].[EmploymentTypeId] = [Position].[EmploymentTypeId]
		LEFT JOIN [StaffingManager].[PositionStatus] WITH (NOLOCK) ON [PositionStatus].[PositionStatusId] = [Position].[PositionStatusId]
		WHERE [Position].[OrganizationId] = @_organizationId '
	IF (SELECT count(*) from @status) > 0
		SET @Where = CONCAT(@Where, 'AND [PositionStatus].[PositionStatusName] IN ( SELECT [StatusName] FROM @_Status) ')
	IF(SELECT count(*) from @type) > 0
		SET @Where = CONCAT(@Where, 'AND [EmploymentType].[EmploymentTypeName] IN (SELECT [TypeName] FROM @_Type) ')
	IF (SELECT count(*) from @tags) > 0
		SET @Where = CONCAT(@Where, 'AND [Tag].[TagName] IN (SELECT [TagName] FROM @_Tags) ')
		SET @sSQL = CONCAT(@sSQL, @where, @order)
	EXEC sp_executesql @sSQL,
		N'@_organizationId INT, @_Status [StaffingManager].[StatusesTable] READONLY, @_Type [StaffingManager].[TypesTable] READONLY, @_Tags [Lookup].[TagTable] READONLY',
	@_organizationId = @organizationId, @_Status = @status, @_Type = @type, @_Tags = @tags

	-- Select all tags from the positions
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
	
	-- Select all Employment Types from the org
		SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [StaffingManager].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId

	-- Select all Position Levels from the org
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [StaffingManager].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId

	-- Select all Position Status' from the org
		SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [StaffingManager].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
	
	-- Select all application Status' from the org
		SELECT [ApplicationStatusId],
		[OrganizationId],
		[ApplicationStatusName]
	FROM [StaffingManager].[ApplicationStatus]
	WHERE [ApplicationStatus].[OrganizationId] = @organizationId
	
	-- Select all Positions Customers from the org
	SELECT [CustomerId],
		   [CustomerName],
		   [AddressId],
		   [ContactEmail],
		   [ContactPhoneNumber],
		   [FaxNumber],
		   [Website],
		   [EIN],
		   [CustomerCreatedUtc],
		   [OrganizationId],
		   [CustomerOrgId],
		   [IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @organizationId
END
GO
PRINT N'Altering [StaffingManager].[UpdatePosition]...';


GO
ALTER PROCEDURE [StaffingManager].[UpdatePosition]
	@positionId INT,
	@organizationId INT,
	@customerId INT,
	@addressId INT,
	@startDate DATETIME2(0),
	@positionStatus INT,
	@positionTitle NVARCHAR(140),
	@billingRateFrequency INT,
	@billingRateAmount INT,
	@durationMonths INT,
	@employmentType INT,
	@positionCount INT,
	@requiredSkills NVARCHAR (MAX),
	@jobResponsibilities NVARCHAR (MAX),
	@desiredSkills NVARCHAR (MAX),
	@positionLevel NVARCHAR (140),
	@hiringManager NVARCHAR (140),
	@teamName NVARCHAR (140),
	@address1 NVARCHAR (64),
	@address2 NVARCHAR (64),
	@city NVARCHAR(32),
	@stateId NVARCHAR(32),
	@countryCode NVARCHAR(32),
	@postalCode NVARCHAR(16),
	@tags [Lookup].[TagTable] READONLY

AS
BEGIN
	UPDATE [StaffingManager].[Position] 
	SET [OrganizationId] = @organizationId, 
		[CustomerId] = @customerId, 
		[AddressId] = @addressId,
		[StartDate] = @startDate, 
		[PositionStatusId] = @positionStatus, 
		[PositionTitle] = @positionTitle,
		[BillingRateFrequency] = @billingRateFrequency,
		[BillingRateAmount] = @billingRateAmount,
		[DurationMonths] = @durationMonths,
		[EmploymentTypeId] = @employmentType,
		[PositionCount] = @positionCount,
		[RequiredSkills] = @requiredSkills,
		[JobResponsibilities] = @jobResponsibilities,
		[DesiredSkills] = @desiredSkills,
		[PositionLevelId] = @positionLevel,
		[HiringManager] = @hiringManager,
		[TeamName] = @teamName,
		[PositionModifiedUtc] = SYSUTCDATETIME()
	WHERE [PositionId] = @positionId

	SET NOCOUNT ON
	EXEC [Lookup].[UpdateAddress]
		@addressId,
		@address1,
		@address2,
		@city,
		@stateId,
		@postalCode,
		@countryCode

END
GO
PRINT N'Creating [Auth].[GetOrganizationUsers]...';


GO
create procedure Auth.GetOrganizationUsers
	@orgId int
as
begin
	set nocount on
		-- get user information with address
	select u.*, a.*, ou.*, s.StateName as 'State', c.CountryName as 'Country' from [User] u with (nolock)
	inner join OrganizationUser ou with (nolock) on ou.UserId = u.UserId
	inner join Organization o with (nolock) on o.OrganizationId = ou.OrganizationId
	left join [Lookup].[Address] a with (nolock) on a.AddressId = u.AddressId
	left join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	left join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where ou.OrganizationId = @orgId and o.IsActive = 1
end
GO
PRINT N'Creating [Auth].[GetProductRoles]...';


GO
create procedure Auth.GetProductRoles
	@orgId int,
	@productId int
as
begin
	set nocount on
	-- NOTE: IGNORE orgId for now, but later we need to use it
	select * from ProductRole with (nolock)
	where ProductId = @productId
end
GO
PRINT N'Creating [Auth].[UpdateEmployeeIdAndOrgRole]...';


GO
create procedure Auth.UpdateEmployeeIdAndOrgRole
	@orgId int,
	@userId int,
	@employeeId nvarchar(16),
	@orgRoleId int
as
begin
	set nocount on
	Update OrganizationUser set EmployeeId = @employeeId, OrganizationRoleId = @orgRoleId
	where OrganizationId = @orgId and UserId = @userId
	select @@ROWCOUNT
end
GO
PRINT N'Creating [Billing].[GetAllSkus]...';


GO
create procedure Billing.GetAllSkus
as
begin
	set nocount on
	select * from Sku with (nolock)
end
GO
PRINT N'Creating [Billing].[GetSubscriptions]...';


GO
CREATE PROCEDURE [Billing].[GetSubscriptions]
    @orgId INT
AS
BEGIN
    SET NOCOUNT ON
    SELECT [s].*,
           [sk].[SkuName],
		   [sk].[IconUrl],
           [p].[ProductId],
           [p].[ProductName],
           [p].[AreaUrl],
           [p].[Description] AS [ProductDescription]
      FROM [Subscription] [s] WITH (NOLOCK)
      JOIN [Sku]         [sk] WITH (NOLOCK) ON [sk].[SkuId] = [s].[SkuId]
      JOIN [Product]      [p] WITH (NOLOCK) ON [p].[ProductId] = [sk].[ProductId]
     WHERE [s].[OrganizationId] = @orgId
       AND [s].[IsActive] = 1
       AND [sk].[IsActive] = 1
       AND [p].[IsActive] = 1
END
GO
PRINT N'Creating [Billing].[UpdateSubscriptionName]...';


GO
-- TODO: pass in subscriptionId as a parameter to simplify logic

create PROCEDURE [Billing].[UpdateSubscriptionName]
	@subscriptionId INT,
	@subscriptionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Billing].[Subscription] SET [SubscriptionName] = @subscriptionName
	where [Subscription].SubscriptionId = @subscriptionId and Subscription.IsActive = 1
END
GO
PRINT N'Creating [Billing].[UpdateSubscriptionSkuAndName]...';


GO
create procedure Billing.UpdateSubscriptionSkuAndName
	@subscriptionId int,
	@subscriptionName nvarchar(64),
	@skuId int
as
begin
	set nocount on
	update Subscription set SubscriptionName = @subscriptionName, SkuId = @skuId
	where SubscriptionId = @subscriptionId
end
GO
PRINT N'Creating [Lookup].[GetAllStates]...';


GO
create procedure Lookup.GetAllStates
as
begin
	set nocount on
	select * from State with (nolock)
end
GO
PRINT N'Creating [StaffingManager].[GetApplicantsByOrgId]...';


GO
CREATE PROCEDURE [StaffingManager].[GetApplicantsByOrgId]
	@orgId INT
AS
BEGIN
	SELECT * FROM [StaffingManager].[Applicant] WHERE [OrganizationId] = @orgId
END
GO
PRINT N'Creating [StaffingManager].[GetFullApplicationInfosByPositionId]...';


GO
CREATE PROCEDURE [StaffingManager].[GetFullApplicationInfosByPositionId]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Application].[ApplicationId],
		[Application].[ApplicantId],
		[Application].[ApplicationStatusId],
		[Application].[ApplicationModifiedUtc],
		[Application].[Notes]
	FROM [StaffingManager].[Application] WHERE [PositionId] = @positionId
	
	SELECT 
		[Applicant].[ApplicantId],
		[Applicant].[FirstName],
		[Applicant].[LastName],
		[Address].[City],
		[Address].[CountryCode],
		[Address].[StateId],
		[Applicant].[Email],
		[Applicant].[PhoneNumber],
		[Applicant].[Notes]
	FROM [StaffingManager].[Application] 
		Join [StaffingManager].[Applicant] on [Application].[ApplicantId] = [Applicant].[ApplicantId]
		Join [Lookup].[Address] on [Applicant].[AddressId] = [Address].[AddressId]
		WHERE [Application].[PositionId] = @positionId
		
	SELECT 
		[ApplicationDocument].[ApplicationId],
		[ApplicationDocument].[ApplicationDocumentId],
		[ApplicationDocument].[DocumentLink],
		[ApplicationDocument].[DocumentName]
	FROM [StaffingManager].[ApplicationDocument] 
		Join [StaffingManager].[Application] on [Application].[ApplicationId] = [ApplicationDocument].[ApplicationId]
		WHERE [Application].[PositionId] = @positionId
END
GO
PRINT N'Creating [TimeTracker].[GetOldLockDate]...';


GO
CREATE PROCEDURE [TimeTracker].[GetOldLockDate]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity] FROM [TimeTracker].[Setting] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;
GO
PRINT N'Creating [TimeTracker].[UpdateOldLockDate]...';


GO
CREATE PROCEDURE [TimeTracker].[UpdateOldLockDate]
	@organizationId INT,
	@isLockDateUsed BIT,
	@lockDatePeriod INT,
	@lockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [IsLockDateUsed] = @isLockDateUsed,
			[LockDatePeriod] = @lockDatePeriod,
			[LockDateQuantity] = @lockDateQuantity
		WHERE [OrganizationId] = @organizationId;
GO
PRINT N'Creating [TimeTracker].[UpdatePayrollProcessedDate]...';


GO
CREATE PROCEDURE [TimeTracker].[UpdatePayrollProcessedDate]
	@organizationId INT,
	@payrollProcessedDate DATE
AS
	UPDATE [TimeTracker].[Setting]
	SET [PayrollProcessedDate] = @payrollProcessedDate
	WHERE [OrganizationId] = @organizationId;
GO
PRINT N'Creating [TimeTracker].[UpdateTimeEntryStatusById]...';


GO
CREATE PROCEDURE [TimeTracker].[UpdateTimeEntryStatusById]
    @timeEntryId INT,
    @timeEntryStatusId INT
AS
    UPDATE [t]
       SET [t].[TimeEntryStatusId] = @timeEntryStatusId
      FROM [TimeTracker].[TimeEntry] [t]
     WHERE [t].[TimeEntryId] = @timeEntryId
GO
PRINT N'Refreshing [Auth].[GetMaxEmployeeId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetMaxEmployeeId]';


GO
PRINT N'Refreshing [Finance].[UpdateAccount]...';


GO
EXECUTE sp_refreshsqlmodule N'[Finance].[UpdateAccount]';


GO
PRINT N'Refreshing [StaffingManager].[DeleteApplicant]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[DeleteApplicant]';


GO
PRINT N'Refreshing [StaffingManager].[GetApplicantByApplicationId]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[GetApplicantByApplicationId]';


GO
PRINT N'Refreshing [StaffingManager].[UpdateApplicant]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[UpdateApplicant]';


GO
PRINT N'Refreshing [StaffingManager].[CreateStaffingSettings]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[CreateStaffingSettings]';


GO
PRINT N'Refreshing [StaffingManager].[UpdateStaffingSettings]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[UpdateStaffingSettings]';


GO
PRINT N'Refreshing [TimeTracker].[GetAllSettings]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[GetAllSettings]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateOvertime]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateOvertime]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateSettings]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateSettings]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateStartOfWeek]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateStartOfWeek]';


GO
PRINT N'Refreshing [Hrm].[DeleteHoliday]...';


GO
EXECUTE sp_refreshsqlmodule N'[Hrm].[DeleteHoliday]';


GO
PRINT N'Refreshing [TimeTracker].[DeleteTimeEntry]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[DeleteTimeEntry]';


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '90d6ec8a-d404-49d2-8c63-f620d5bd94c1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('90d6ec8a-d404-49d2-8c63-f620d5bd94c1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'f04822d5-3808-4dc7-92dc-10743a712f58')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('f04822d5-3808-4dc7-92dc-10743a712f58')

GO

GO
PRINT N'Checking existing data against newly created constraints';





GO
ALTER TABLE [Auth].[Invitation] WITH CHECK CHECK CONSTRAINT [FK_Invitation_OrganizationId];

ALTER TABLE [Finance].[Account] WITH CHECK CHECK CONSTRAINT [FK_Account_Account];

ALTER TABLE [Finance].[Account] WITH CHECK CHECK CONSTRAINT [FK_Account_AccountType];

ALTER TABLE [Finance].[Account] WITH CHECK CHECK CONSTRAINT [FK_Billing_SubId];

ALTER TABLE [StaffingManager].[Applicant] WITH CHECK CHECK CONSTRAINT [FK_Applicant_Address];

ALTER TABLE [StaffingManager].[Application] WITH CHECK CHECK CONSTRAINT [FK_Application_Applicant];

ALTER TABLE [TimeTracker].[TimeEntry] WITH CHECK CHECK CONSTRAINT [FK_TimeEntry_Project];

ALTER TABLE [TimeTracker].[TimeEntry] WITH CHECK CHECK CONSTRAINT [FK_TimeEntry_User];

ALTER TABLE [StaffingManager].[StaffingSettings] WITH CHECK CHECK CONSTRAINT [FK_DefaultApplicationStatus];


GO
PRINT N'Update complete.';


GO

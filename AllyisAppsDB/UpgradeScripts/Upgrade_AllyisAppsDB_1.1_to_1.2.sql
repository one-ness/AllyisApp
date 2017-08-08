
GO
PRINT N'Dropping [Auth].[FK_User_Language]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [FK_User_Language];


GO
PRINT N'Dropping [Billing].[UpdateSubscriptionName]...';


GO
DROP PROCEDURE [Billing].[UpdateSubscriptionName];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Altering [Auth].[User]...';


GO
ALTER TABLE [Auth].[User] ALTER COLUMN [PreferredLanguageId] VARCHAR (16) NULL;


GO
PRINT N'Starting rebuilding table [Lookup].[Language]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Lookup].[tmp_ms_xx_Language] (
    [LanguageName] NVARCHAR (64) NOT NULL,
    [CultureName]  VARCHAR (16)  NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Language1] PRIMARY KEY CLUSTERED ([CultureName] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Lookup].[Language])
    BEGIN
        INSERT INTO [Lookup].[tmp_ms_xx_Language] ([CultureName], [LanguageName])
        SELECT   [CultureName],
                 [LanguageName]
        FROM     [Lookup].[Language]
        ORDER BY [CultureName] ASC;
    END

DROP TABLE [Lookup].[Language];

EXECUTE sp_rename N'[Lookup].[tmp_ms_xx_Language]', N'Language';

EXECUTE sp_rename N'[Lookup].[tmp_ms_xx_constraint_PK_Language1]', N'PK_Language', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Lookup].[Language].[IX_Language]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Language]
    ON [Lookup].[Language]([CultureName] ASC);


GO
PRINT N'Creating [Billing].[Subscription].[IX_Subscription_SkuId_OrganizationId]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Subscription_SkuId_OrganizationId]
    ON [Billing].[Subscription]([SkuId] ASC, [OrganizationId] ASC);


GO
PRINT N'Creating [Auth].[FK_User_Language]...';


GO
ALTER TABLE [Auth].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Language] FOREIGN KEY ([PreferredLanguageId]) REFERENCES [Lookup].[Language] ([CultureName]) ON DELETE SET DEFAULT;


GO
PRINT N'Altering [Auth].[CreateUser]...';


GO
ALTER PROCEDURE [Auth].[CreateUser]
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
    @address NVARCHAR(100), 
    @city NVARCHAR(32), 
    @state NVARCHAR(32), 
    @country NVARCHAR(32), 
    @postalCode NVARCHAR(16),
	@email NVARCHAR(256), 
    @phoneNumber VARCHAR(32),
	@dateOfBirth DATETIME2(0),
	@passwordHash NVARCHAR(MAX),
	@emailConfirmationCode UNIQUEIdENTIFIER,
	@isTwoFactorEnabled BIT,
	@isLockoutEnabled BIT,
	@lockoutEndDateUtc DATE,
	@CultureName VARCHAR (16)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Lookup].[Address]
		([Address1],
		[City],
		[StateId],
		[CountryId],
		[PostalCode])
	VALUES
		(@address,
		@city,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		@postalCode);

	INSERT INTO [Auth].[User] 
		([FirstName], 
		[LastName], 
		[AddressId],
		[Email], 
		[PhoneNumber], 
		[DateOfBirth],
		[PasswordHash],
		[EmailConfirmationCode],
		[IsEmailConfirmed],
		[IsTwoFactorEnabled],
		[AccessFailedCount],
		[IsLockoutEnabled],
		[LockoutEndDateUtc],
		[PreferredLanguageId])
	VALUES 
		(@firstName, 
		@lastName,
		SCOPE_IDENTITY(),
		@email,
		@phoneNumber,
		@dateOfBirth, 
		@passwordHash, 
		@emailConfirmationCode,
		0,
		COALESCE(@isTwoFactorEnabled,0),
		0,
		COALESCE(@isLockoutEnabled,0),
		COALESCE(@lockoutEndDateUtc,NULL),
		@CultureName);

	SELECT SCOPE_IDENTITY();
END
GO
PRINT N'Altering [Auth].[UpdateUserLanguagePreference]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserLanguagePreference]
	@id INT,
	@CultureName VARCHAR (16)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [PreferredLanguageId] = @CultureName
	WHERE [UserId] = @id
END
GO
PRINT N'Altering [Billing].[UpdateSubscription]...';


GO
-- TODO: pass in subscriptionId as a parameter to simplify logic

ALTER PROCEDURE [Billing].[UpdateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
	--Because the productRoleId and subscriptionId don't change, no need to update SubscriptionUser table
	UPDATE [Billing].[Subscription] SET [SkuId] = @skuId, [SubscriptionName] = @subscriptionName
		WHERE [OrganizationId] = @organizationId
		AND [Subscription].[IsActive] = 1
		AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
						WHERE [SkuId] != @skuId
						AND [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @skuId)
						AND [OrganizationId] = @organizationId);
END
GO
PRINT N'Altering [Lookup].[GetLanguageById]...';


GO
ALTER PROCEDURE [Lookup].[GetLanguageById]
	@CultureName VARCHAR (16)
AS
BEGIN
	SELECT
		[Language].[LanguageName],
		[Language].[CultureName]
	FROM [Lookup].[Language] WITH (NOLOCK)
	WHERE [Language].[CultureName] = @CultureName
	 
END
GO
PRINT N'Altering [Lookup].[GetLanguages]...';


GO
ALTER PROCEDURE [Lookup].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[CultureName],
		[LanguageName]
	FROM [Language] WITH (NOLOCK)
END
GO
PRINT N'Altering [Auth].[DeleteOrgUser]...';


GO
ALTER PROCEDURE [Auth].[DeleteOrgUser]
	@organizationId INT,
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Pjm].[ProjectUser] WHERE [UserId] = @userId AND [ProjectId] IN 
		(SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN
			(SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = @organizationId));

	DELETE FROM [Billing].[SubscriptionUser] WHERE [UserId] = @userId AND [SubscriptionId] IN 
		(SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] = @organizationId);
	
	DELETE FROM [Auth].[OrganizationUser] WHERE [OrganizationUser].[UserId] = @userId AND [OrganizationUser].[OrganizationId] = @organizationId

	
	--DECLARE @subscriptionId INT = 
	--	(SELECT TOP 1 ([SubscriptionId]) 
	--	FROM [Billing].[Subscription] 
	--	WITH (NOLOCK)
	--	WHERE [OrganizationId] = @organizationId 
	--	ORDER BY [SubscriptionId] DESC);
	--DELETE FROM [Billing].[SubscriptionUser] WHERE [SubscriptionId] = @subscriptionId AND [UserId] = @userId
END
GO
PRINT N'Creating [Billing].[CreateSubscription]...';


GO
CREATE PROCEDURE [Billing].[CreateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50),
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	--Create the new subscription
	INSERT INTO [Billing].[Subscription]
		([OrganizationId],
		[SkuId],
		[SubscriptionName])
	VALUES
		(@organizationId,
		@skuId,
		@subscriptionName);

	DECLARE @subscriptionId INT = IDENT_CURRENT('[Billing].[Subscription]');

	-- Insert all members of given org to SubscriptionUser table with User role
	-- ASSUMPTION: 1 is the productRoleId of "User" for all subscriptions
	INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
		SELECT [UserId], @subscriptionId, 1
		FROM [Auth].[OrganizationUser]
		WHERE [OrganizationId] = @organizationId;

	-- Update the current user's role to manager
	-- ASSUMPTION: 2 is the productRoleId of "Manager" for all subscriptions
	EXEC [Billing].[UpdateSubscriptionUserProductRole] 2, @subscriptionId, @userId

	SELECT @subscriptionId;
END
GO
PRINT N'Refreshing [Auth].[AcceptInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[AcceptInvitation]';


GO
PRINT N'Refreshing [Auth].[GetOrgAndSubRoles]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgAndSubRoles]';


GO
PRINT N'Refreshing [Auth].[GetOrganizationOwnerEmails]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrganizationOwnerEmails]';


GO
PRINT N'Refreshing [Auth].[GetOrgManagementInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgManagementInfo]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserByEmail]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserByEmail]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserList]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserList]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserRole]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserRole]';


GO
PRINT N'Refreshing [Auth].[GetPasswordHashFromUserId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetPasswordHashFromUserId]';


GO
PRINT N'Refreshing [Auth].[GetRolesAndPermissions]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetRolesAndPermissions]';


GO
PRINT N'Refreshing [Auth].[GetUserContextInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserContextInfo]';


GO
PRINT N'Refreshing [Auth].[GetUserFromEmail]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserFromEmail]';


GO
PRINT N'Refreshing [Auth].[GetUserInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserInfo]';


GO
PRINT N'Refreshing [Auth].[GetUserOrgsAndInvitationInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserOrgsAndInvitationInfo]';


GO
PRINT N'Refreshing [Auth].[GetUsersWithSubscriptionToProductInOrganization]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUsersWithSubscriptionToProductInOrganization]';


GO
PRINT N'Refreshing [Auth].[InviteUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[InviteUser]';


GO
PRINT N'Refreshing [Auth].[RemoveInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[RemoveInvitation]';


GO
PRINT N'Refreshing [Auth].[UpdateEmailConfirmed]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateEmailConfirmed]';


GO
PRINT N'Refreshing [Auth].[UpdateUserActiveOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserActiveOrg]';


GO
PRINT N'Refreshing [Auth].[UpdateUserActiveSub]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserActiveSub]';


GO
PRINT N'Refreshing [Auth].[UpdateUserInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserInfo]';


GO
PRINT N'Refreshing [Auth].[UpdateUserPassword]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserPassword]';


GO
PRINT N'Refreshing [Auth].[UpdateUserPasswordResetCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserPasswordResetCode]';


GO
PRINT N'Refreshing [Auth].[UpdateUserPasswordUsingCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserPasswordUsingCode]';


GO
PRINT N'Refreshing [Billing].[GetBillingHistoryByOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetBillingHistoryByOrg]';


GO
PRINT N'Refreshing [Pjm].[GetNextProjectIdAndSubUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetNextProjectIdAndSubUsers]';


GO
PRINT N'Refreshing [Pjm].[GetProjectEditInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetProjectEditInfo]';


GO
PRINT N'Refreshing [Pjm].[GetProjectsForOrgAndUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetProjectsForOrgAndUser]';


GO
PRINT N'Refreshing [TimeTracker].[GetReportInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[GetReportInfo]';


GO
PRINT N'Refreshing [TimeTracker].[GetTimeEntriesByUserOverDateRange]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[GetTimeEntriesByUserOverDateRange]';


GO
PRINT N'Refreshing [TimeTracker].[GetTimeEntriesOverDateRange]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[GetTimeEntriesOverDateRange]';


GO
PRINT N'Refreshing [TimeTracker].[GetTimeEntryIndexInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[GetTimeEntryIndexInfo]';


GO
PRINT N'Checking existing data against newly created constraints';

GO
PRINT N'Update complete.';


GO

alter table [Auth].[User]
nocheck constraint FK_User_Language

UPDATE [Auth].[User]
SET [PreferredLanguageId] = 'en-Us'
WHERE [PreferredLanguageId] = 1;

alter table [Auth].[User]
check constraint FK_User_Language

GO
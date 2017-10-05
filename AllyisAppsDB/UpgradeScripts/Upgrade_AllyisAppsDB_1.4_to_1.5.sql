
/*
The column [Auth].[User].[MaxAmount] is being dropped, data loss could occur.
*/


GO
PRINT N'Rename refactoring operation with key 585735d4-c544-470c-8887-384ce93c34b4 is skipped, element [Billing].[Sku].[SkuIconUrl] (SqlSimpleColumn) will not be renamed to IconUrl';


GO
PRINT N'Dropping [Auth].[DF_OrganizationUser_CreatedUtc]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [DF_OrganizationUser_CreatedUtc];


GO
PRINT N'Dropping [StaffingManager].[DF_Position_EmploymentType]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [DF_Position_EmploymentType];


GO
PRINT N'Dropping [StaffingManager].[DF_Position_PositionLevel]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [DF_Position_PositionLevel];


GO
PRINT N'Dropping [StaffingManager].[DF_Position_PositionStatus]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [DF_Position_PositionStatus];


GO
PRINT N'Dropping [Auth].[DF__User__MaxAmount]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__MaxAmount];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_Organization]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_Organization];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_OrganizationRole]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_OrganizationRole];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_User]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_User];


GO
PRINT N'Dropping [StaffingManager].[FK_Position_EmploymentType]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [FK_Position_EmploymentType];


GO
PRINT N'Dropping [StaffingManager].[FK_Position_PositionLevel]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [FK_Position_PositionLevel];


GO
PRINT N'Dropping [StaffingManager].[FK_Position_PositionStatus]...';


GO
ALTER TABLE [StaffingManager].[Position] DROP CONSTRAINT [FK_Position_PositionStatus];


GO
PRINT N'Dropping [StaffingManager].[GetStaffingIndexInfoFiltered]...';


GO
DROP PROCEDURE [StaffingManager].[GetStaffingIndexInfoFiltered];


GO
PRINT N'Dropping [StaffingManager].[SetupPosition]...';


GO
DROP PROCEDURE [StaffingManager].[SetupPosition];


GO
PRINT N'Dropping [StaffingManager].[CreatePositionTags]...';


GO
DROP PROCEDURE [StaffingManager].[CreatePositionTags];


GO
PRINT N'Dropping [Lookup].[TagTable]...';


GO
DROP TYPE [Lookup].[TagTable];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Creating [Lookup].[TagTable]...';


GO
CREATE TYPE [Lookup].[TagTable] AS TABLE (
    [TagName] NVARCHAR (64) NULL);


GO
PRINT N'Creating [StaffingManager].[StatusesTable]...';


GO
CREATE TYPE [StaffingManager].[StatusesTable] AS TABLE (
    [StatusName] NVARCHAR (64) NULL);


GO
PRINT N'Creating [StaffingManager].[TypesTable]...';


GO
CREATE TYPE [StaffingManager].[TypesTable] AS TABLE (
    [TypeName] NVARCHAR (64) NULL);


GO
PRINT N'Starting rebuilding table [Auth].[OrganizationUser]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Auth].[tmp_ms_xx_OrganizationUser] (
    [UserId]                     INT           NOT NULL,
    [OrganizationId]             INT           NOT NULL,
    [EmployeeId]                 NVARCHAR (16) NOT NULL,
    [OrganizationRoleId]         INT           NOT NULL,
    [MaxAmount]                  DECIMAL (18)  CONSTRAINT [DF__User__MaxAmount] DEFAULT ((0)) NOT NULL,
    [OrganizationUserCreatedUtc] DATETIME2 (0) CONSTRAINT [DF_OrganizationUser_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_OrganizationUser1] PRIMARY KEY CLUSTERED ([UserId] ASC, [OrganizationId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Auth].[OrganizationUser])
    BEGIN
        INSERT INTO [Auth].[tmp_ms_xx_OrganizationUser] ([UserId], [OrganizationId], [EmployeeId], [OrganizationRoleId], [OrganizationUserCreatedUtc])
        SELECT   [UserId],
                 [OrganizationId],
                 [EmployeeId],
                 [OrganizationRoleId],
                 [OrganizationUserCreatedUtc]
        FROM     [Auth].[OrganizationUser]
        ORDER BY [UserId] ASC, [OrganizationId] ASC;
    END

DROP TABLE [Auth].[OrganizationUser];

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_OrganizationUser]', N'OrganizationUser';

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_constraint_PK_OrganizationUser1]', N'PK_OrganizationUser', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Auth].[OrganizationUser].[IX_OrganizationUser]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser]([UserId] ASC, [OrganizationRoleId] ASC);


GO
PRINT N'Creating [Auth].[OrganizationUser].[IX_OrganizationUser_1]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationUser_1]
    ON [Auth].[OrganizationUser]([OrganizationId] ASC, [EmployeeId] ASC);


GO
PRINT N'Altering [Auth].[User]...';


GO
ALTER TABLE [Auth].[User] DROP COLUMN [MaxAmount];


GO
PRINT N'Altering [Billing].[Sku]...';


GO
ALTER TABLE [Billing].[Sku]
    ADD [IconUrl] NVARCHAR (512) NULL;


GO
PRINT N'Altering [StaffingManager].[Position]...';


GO
ALTER TABLE [StaffingManager].[Position] ALTER COLUMN [EmploymentTypeId] INT NULL;

ALTER TABLE [StaffingManager].[Position] ALTER COLUMN [PositionLevelId] INT NULL;

ALTER TABLE [StaffingManager].[Position] ALTER COLUMN [PositionStatusId] INT NULL;


GO
PRINT N'Creating [StaffingManager].[StaffingSettings]...';


GO
CREATE TABLE [StaffingManager].[StaffingSettings] (
    [StaffingSettingsId]      INT IDENTITY (13332, 3) NOT NULL,
    [OrganizationId]          INT NOT NULL,
    [DefaultPositionStatusId] INT NULL,
    CONSTRAINT [PK_StaffingSettingsId] PRIMARY KEY CLUSTERED ([StaffingSettingsId] ASC)
);


GO
PRINT N'Creating [Crm].[Customer].[IX_OrganizationUser_1]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrganizationUser_1]
    ON [Crm].[Customer]([OrganizationId] ASC, [CustomerOrgId] ASC);


GO
PRINT N'Creating [StaffingManager].[DF_Position_EmploymentType]...';


GO
ALTER TABLE [StaffingManager].[Position]
    ADD CONSTRAINT [DF_Position_EmploymentType] DEFAULT ((0)) FOR [EmploymentTypeId];


GO
PRINT N'Creating [StaffingManager].[DF_Position_PositionLevel]...';


GO
ALTER TABLE [StaffingManager].[Position]
    ADD CONSTRAINT [DF_Position_PositionLevel] DEFAULT ((0)) FOR [PositionLevelId];


GO
PRINT N'Creating [StaffingManager].[DF_Position_PositionStatus]...';


GO
ALTER TABLE [StaffingManager].[Position]
    ADD CONSTRAINT [DF_Position_PositionStatus] DEFAULT ((0)) FOR [PositionStatusId];


GO
PRINT N'Creating unnamed constraint on [StaffingManager].[StaffingSettings]...';


GO
ALTER TABLE [StaffingManager].[StaffingSettings]
    ADD DEFAULT ((0)) FOR [OrganizationId];


GO
PRINT N'Creating [Auth].[FK_OrganizationUser_Organization]...';


GO
ALTER TABLE [Auth].[OrganizationUser] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationUser_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Auth].[FK_OrganizationUser_OrganizationRole]...';


GO
ALTER TABLE [Auth].[OrganizationUser] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationUser_OrganizationRole] FOREIGN KEY ([OrganizationRoleId]) REFERENCES [Auth].[OrganizationRole] ([OrganizationRoleId]);


GO
PRINT N'Creating [Auth].[FK_OrganizationUser_User]...';


GO
ALTER TABLE [Auth].[OrganizationUser] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [StaffingManager].[FK_Position_EmploymentType]...';


GO
ALTER TABLE [StaffingManager].[Position] WITH NOCHECK
    ADD CONSTRAINT [FK_Position_EmploymentType] FOREIGN KEY ([EmploymentTypeId]) REFERENCES [StaffingManager].[EmploymentType] ([EmploymentTypeId]);


GO
PRINT N'Creating [StaffingManager].[FK_Position_PositionLevel]...';


GO
ALTER TABLE [StaffingManager].[Position] WITH NOCHECK
    ADD CONSTRAINT [FK_Position_PositionLevel] FOREIGN KEY ([PositionLevelId]) REFERENCES [StaffingManager].[PositionLevel] ([PositionLevelId]);


GO
PRINT N'Creating [StaffingManager].[FK_Position_PositionStatus]...';


GO
ALTER TABLE [StaffingManager].[Position] WITH NOCHECK
    ADD CONSTRAINT [FK_Position_PositionStatus] FOREIGN KEY ([PositionStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId]);


GO
PRINT N'Creating [StaffingManager].[FK_Organization]...';


GO
ALTER TABLE [StaffingManager].[StaffingSettings] WITH NOCHECK
    ADD CONSTRAINT [FK_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [StaffingManager].[FK_DefaultStatus]...';


GO
ALTER TABLE [StaffingManager].[StaffingSettings] WITH NOCHECK
    ADD CONSTRAINT [FK_DefaultStatus] FOREIGN KEY ([DefaultPositionStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId]);


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
	WHERE [OrganizationId] = @organizationId AND [IsActive] = 1
	ORDER BY [EmployeeId] DESC
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
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @organizationId AND [IsActive] = 1 AND [StatusId] = 0

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

	SELECT 
		[InvitationId], 
		[Invitation].[Email], 
		[Invitation].[FirstName], 
		[Invitation].[LastName],  
		[Invitation].[OrganizationId],
		[Organization].[OrganizationName] AS 'OrganizationName', 
		[OrganizationRoleId],
		[EmployeeId] 
	FROM [Auth].[User] WITH (NOLOCK)
	LEFT JOIN [Auth].[Invitation] WITH (NOLOCK) ON [User].[Email] = [Invitation].[Email]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Invitation].[OrganizationId] = [Organization].[OrganizationId]
	WHERE [User].[UserId] = @userId AND [Invitation].[IsActive] = 1 and [Invitation].[DecisionDateUtc] is null

end
GO
PRINT N'Altering [Auth].[GetUserContext]...';


GO
ALTER procedure [Auth].[GetUserContext]
	@userId int
as
begin
	set nocount on
	-- return 3 result sets
	-- get user information
	select u.FirstName, u.LastName, u.UserId, u.Email, u.PreferredLanguageId from [User] u with (nolock)
	where u.UserId = @userId;

	-- get list of organizations and the user role in each
	create table #OrgAndRole(OrganizationId int, OrganizationRoleId int, OrganizationName nvarchar(64), MaxAmount decimal)
	insert into #OrgAndRole(OrganizationId, OrganizationRoleId, OrganizationName, MaxAmount) select ou.OrganizationId, ou.OrganizationRoleId, o.OrganizationName, ou.MaxAmount from OrganizationUser ou with (nolock)
	inner join Organization o with (nolock) on o.OrganizationId = ou.OrganizationId
	where ou.UserId = @userId and o.IsActive = 1
	select * from #OrgAndRole with (nolock)

	-- get the subscriptions of those organizations and the role of the user in those subscriptions
	select s.SubscriptionId, s.SubscriptionName,
	sku.SkuId, sku.SkuName, p.ProductId, p.ProductName, p.AreaUrl, su.ProductRoleId, s.OrganizationId from Billing.Subscription s with (nolock)
	inner join #OrgAndRole orgrole with (nolock) on orgrole.OrganizationId = s.OrganizationId
	inner join Billing.Sku sku with (nolock) on sku.SkuId = s.SkuId
	inner join Billing.Product p with (nolock) on p.ProductId = sku.ProductId
	inner join Billing.SubscriptionUser su with (nolock) on su.SubscriptionId = s.SubscriptionId
	where s.IsActive = 1 and su.UserId = @userId

	-- drop the temp table
	drop table #OrgAndRole
end
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
PRINT N'Altering [Auth].[GetUsersWithSubscriptionToProductInOrganization]...';


GO
ALTER PROCEDURE [Auth].[GetUsersWithSubscriptionToProductInOrganization]
	@organizationId INT,
	@productId INT 
AS
BEGIN
	SET NOCOUNT ON;
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
END
GO
PRINT N'Altering [Auth].[UpdateUserMaxAmount]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserMaxAmount]
	@userId int,
	@orgId int,
	@maxAmount decimal
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Auth].[OrganizationUser]
	SET [MaxAmount] = @maxAmount
	WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
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
PRINT N'Creating [StaffingManager].[GetStaffingIndexInfoFiltered]...';


GO
CREATE PROCEDURE [StaffingManager].[GetStaffingIndexInfoFiltered]
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
PRINT N'Altering [Auth].[RejectInvitation]...';


GO
ALTER PROCEDURE [Auth].[RejectInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET    [StatusId] = -1, DecisionDateUtc = GETUTCDATE()
	WHERE [InvitationId] = @invitationId;

	SELECT @@ROWCOUNT;
END
GO
PRINT N'Altering [Finance].[CreateAccount]...';


GO
ALTER PROCEDURE [Finance].[CreateAccount]
	@accountName NVARCHAR(100),
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
				[IsActive], 
				[AccountTypeId], 
				[ParentAccountId])
		VALUES (@accountName,
				@isActive,
				@accountTypeId,
				@parentAccountId);

		SET @returnValue = SCOPE_IDENTITY();
	END
END
GO
PRINT N'Altering [Finance].[UpdateAccount]...';


GO
ALTER PROCEDURE [Finance].[UpdateAccount]
	@accountId INT,
	@accountName NVARCHAR(100),
	@isActive BIT,
	@accountTypeId INT,
	@parentAccountId INT,
	@returnValue INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN
		-- Create account
		UPDATE [Finance].[Account]
		SET [AccountName]		= @accountName, 
			[IsActive]			= @isActive, 
			[AccountTypeId]		= @accountTypeId, 
			[ParentAccountId]	= @parentAccountId
		WHERE [AccountId]		= @accountId

		SET @returnValue = 1;
	END
END
GO
PRINT N'Creating [StaffingManager].[CreatePositionTags]...';


GO
CREATE PROCEDURE [StaffingManager].[CreatePositionTags]
	@tags [Lookup].[TagTable] READONLY,
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRANSACTION
		-- COMMENT: Insert the tags into [Tag] if they don't already exist there
		INSERT INTO [Lookup].[Tag] ([TagName])
		SELECT [NEWTAGS].[TagName]
		FROM @tags AS [NEWTAGS]
		WHERE NOT EXISTS
			(SELECT [TagName]
			FROM [Lookup].[Tag]
			WHERE [Tag].[TagName] = [NEWTAGS].[TagName])

		-- Get the tag ids of the newly created tags from the previous statement
		DECLARE @tagIds TABLE ([TagId] INT)
		INSERT INTO @tagIds
		SELECT [TagId]
		FROM [Lookup].[Tag]
		WHERE [Tag].[TagName] IN
			(SELECT [TagName] FROM @tags)
	
		-- Insert all the new tags into [PositionTag]
		INSERT INTO [StaffingManager].[PositionTag] ([PositionId], [TagId])
		SELECT @positionId, [TagId]
		FROM @tagIds
	COMMIT TRANSACTION
END
GO
PRINT N'Creating [Auth].[GetOrgUserMaxAmount]...';


GO
CREATE PROCEDURE [Auth].[GetOrgUserMaxAmount]
	@userId INT, 
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [MaxAmount]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	WHERE [OrganizationId] = @orgId AND [UserId] = @userId;
END
GO
PRINT N'Creating [StaffingManager].[CreateStaffingSettings]...';


GO
CREATE PROCEDURE [StaffingManager].[CreateStaffingSettings]
	@organizationId		INT

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [StaffingManager].[StaffingSettings] 
		([OrganizationId],
		[DefaultPositionStatusId])
	VALUES 	 
		(@organizationId,
		NULL)
END
GO
PRINT N'Creating [StaffingManager].[GetStaffingDefaultStatus]...';


GO
CREATE PROCEDURE [StaffingManager].[GetStaffingDefaultStatus]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [DefaultPositionStatusId]
	FROM [StaffingManager].[StaffingSettings]
	WHERE [StaffingSettings].[OrganizationId] = @organizationId
END
GO
PRINT N'Creating [StaffingManager].[UpdateStaffingSettings]...';


GO
CREATE PROCEDURE [StaffingManager].[UpdateStaffingSettings]
	@organizationId INT,
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [StaffingManager].[StaffingSettings] 
	SET 
		[DefaultPositionStatusId] = @positionStatusId
	WHERE [StaffingSettings].[OrganizationId] = @organizationId
END
GO
PRINT N'Creating [StaffingManager].[SetupPosition]...';


GO
CREATE PROCEDURE [StaffingManager].[SetupPosition]
	@organizationId INT,
	@customerId INT,
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
	@stateId SMALLINT,
	@postalCode NVARCHAR(16),
	@countryCode VARCHAR(8),
	@tags [Lookup].[TagTable] READONLY
AS
BEGIN TRANSACTION

	EXEC [Lookup].[CreateAddress]
		@address1,
		@address2,
		@city,
		@stateId,
		@postalCode,
		@countryCode
		
		DECLARE @addressId INT
		SET @addressId = IDENT_CURRENT('[Lookup].[Address]')

	EXEC [StaffingManager].[CreatePosition]
		@organizationId,
		@customerId,
		@addressId,  
		@startDate, 
		@positionStatus,
		@positionTitle, 
		@billingRateFrequency,
		@billingRateAmount,
		@durationMonths,
		@employmentType,
		@positionCount,
		@requiredSkills,
		@jobResponsibilities,
		@desiredSkills,
		@positionLevel,
		@hiringManager,
		@teamName
	
		DECLARE @positionId INT
		SET @positionId = IDENT_CURRENT('[StaffingManager].[Position]')

	EXEC [StaffingManager].[CreatePositionTags]
		@tags,
		@positionId

	SELECT @positionId
COMMIT TRANSACTION
GO
PRINT N'Refreshing [Auth].[AcceptInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[AcceptInvitation]';


GO
PRINT N'Refreshing [Auth].[CreateInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[CreateInvitation]';


GO
PRINT N'Refreshing [Auth].[CreateOrganizationUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[CreateOrganizationUser]';


GO
PRINT N'Refreshing [Auth].[DeleteOrganizationUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[DeleteOrganizationUsers]';


GO
PRINT N'Refreshing [Auth].[DeleteOrgUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[DeleteOrgUser]';


GO
PRINT N'Refreshing [Auth].[GetOrgAndSubRoles]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgAndSubRoles]';


GO
PRINT N'Refreshing [Auth].[GetOrganizationOwnerEmails]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrganizationOwnerEmails]';


GO
PRINT N'Refreshing [Auth].[GetOrganizationsByUserId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrganizationsByUserId]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserByEmail]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserByEmail]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserEmployeeId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserEmployeeId]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserList]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserList]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserRole]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserRole]';


GO
PRINT N'Refreshing [Auth].[GetOrgWithNextEmployeeId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgWithNextEmployeeId]';


GO
PRINT N'Refreshing [Auth].[GetRolesAndPermissions]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetRolesAndPermissions]';


GO
PRINT N'Refreshing [Auth].[GetUserContextInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserContextInfo]';


GO
PRINT N'Refreshing [Auth].[GetUserOrgsAndInvitationInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserOrgsAndInvitationInfo]';


GO
PRINT N'Refreshing [Auth].[UpdateMember]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateMember]';


GO
PRINT N'Refreshing [Auth].[UpdateOrganizationUsersRole]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateOrganizationUsersRole]';


GO
PRINT N'Refreshing [Billing].[CreateSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[CreateSubscription]';


GO
PRINT N'Refreshing [Pjm].[GetNextProjectIdAndSubUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetNextProjectIdAndSubUsers]';


GO
PRINT N'Refreshing [Pjm].[GetProjectEditInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetProjectEditInfo]';


GO
PRINT N'Refreshing [Pjm].[GetProjectsByUserAndOrganization]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetProjectsByUserAndOrganization]';


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
PRINT N'Refreshing [Auth].[SetupOrganization]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[SetupOrganization]';


GO
PRINT N'Refreshing [Auth].[CreateUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[CreateUser]';


GO
PRINT N'Refreshing [Auth].[GetPasswordHashFromUserId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetPasswordHashFromUserId]';


GO
PRINT N'Refreshing [Auth].[GetUserFromEmail]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserFromEmail]';


GO
PRINT N'Refreshing [Auth].[GetUserInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserInfo]';


GO
PRINT N'Refreshing [Auth].[GetUserProfile]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserProfile]';


GO
PRINT N'Refreshing [Auth].[UpdateEmailConfirmed]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateEmailConfirmed]';


GO
PRINT N'Refreshing [Auth].[UpdateUserActiveSub]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserActiveSub]';


GO
PRINT N'Refreshing [Auth].[UpdateUserInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserInfo]';


GO
PRINT N'Refreshing [Auth].[UpdateUserLanguagePreference]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserLanguagePreference]';


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
PRINT N'Refreshing [Auth].[UpdateUserProfile]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserProfile]';


GO
PRINT N'Refreshing [Billing].[GetBillingHistoryByOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetBillingHistoryByOrg]';


GO
PRINT N'Refreshing [Pjm].[GetProjectsForOrgAndUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetProjectsForOrgAndUser]';


GO
PRINT N'Refreshing [Auth].[GetActiveProductRoleForUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetActiveProductRoleForUser]';


GO
PRINT N'Refreshing [Billing].[DeleteSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[DeleteSubscription]';


GO
PRINT N'Refreshing [Billing].[DeleteSubscriptionUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[DeleteSubscriptionUsers]';


GO
PRINT N'Refreshing [Billing].[GetAllActiveProductsAndSkus]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetAllActiveProductsAndSkus]';


GO
PRINT N'Refreshing [Billing].[GetOrgSkus]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetOrgSkus]';


GO
PRINT N'Refreshing [Billing].[GetProductAreaBySubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetProductAreaBySubscription]';


GO
PRINT N'Refreshing [Billing].[GetProductRolesFromSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetProductRolesFromSubscription]';


GO
PRINT N'Refreshing [Billing].[GetProductSubscriptionInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetProductSubscriptionInfo]';


GO
PRINT N'Refreshing [Billing].[GetSkuById]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetSkuById]';


GO
PRINT N'Refreshing [Billing].[GetSubscriptionsDisplayByOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetSubscriptionsDisplayByOrg]';


GO
PRINT N'Refreshing [Billing].[UpdateSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[UpdateSubscription]';


GO
PRINT N'Refreshing [Billing].[UpdateSubscriptionUserRoles]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[UpdateSubscriptionUserRoles]';


GO
PRINT N'Refreshing [TimeTracker].[CreateBulkTimeEntry]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[CreateBulkTimeEntry]';


GO
PRINT N'Refreshing [Hrm].[CreateHoliday]...';


GO
EXECUTE sp_refreshsqlmodule N'[Hrm].[CreateHoliday]';


GO
PRINT N'Refreshing [StaffingManager].[CreatePosition]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[CreatePosition]';


GO
PRINT N'Refreshing [StaffingManager].[DeletePosition]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[DeletePosition]';


GO
PRINT N'Refreshing [StaffingManager].[GetPosition]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[GetPosition]';


GO
PRINT N'Refreshing [StaffingManager].[GetPositionsByorganizationId]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[GetPositionsByorganizationId]';


GO
PRINT N'Refreshing [StaffingManager].[UpdatePosition]...';


GO
EXECUTE sp_refreshsqlmodule N'[StaffingManager].[UpdatePosition]';


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '585735d4-c544-470c-8887-384ce93c34b4')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('585735d4-c544-470c-8887-384ce93c34b4')

GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [Auth].[OrganizationUser] WITH CHECK CHECK CONSTRAINT [FK_OrganizationUser_Organization];

ALTER TABLE [Auth].[OrganizationUser] WITH CHECK CHECK CONSTRAINT [FK_OrganizationUser_OrganizationRole];

ALTER TABLE [Auth].[OrganizationUser] WITH CHECK CHECK CONSTRAINT [FK_OrganizationUser_User];

ALTER TABLE [StaffingManager].[Position] WITH CHECK CHECK CONSTRAINT [FK_Position_EmploymentType];

ALTER TABLE [StaffingManager].[Position] WITH CHECK CHECK CONSTRAINT [FK_Position_PositionLevel];

ALTER TABLE [StaffingManager].[Position] WITH CHECK CHECK CONSTRAINT [FK_Position_PositionStatus];

ALTER TABLE [StaffingManager].[StaffingSettings] WITH CHECK CHECK CONSTRAINT [FK_Organization];

ALTER TABLE [StaffingManager].[StaffingSettings] WITH CHECK CHECK CONSTRAINT [FK_DefaultStatus];


GO
PRINT N'Update complete.';


GO

UPDATE [Billing].[Sku] SET [IconUrl] = 'Content/TimeTracker/icons/TimeTracker.png' WHERE [SkuId] = 200001;

Go
-- Expense Tracker Skus --
Update  [Billing].[Sku] SET [IconUrl] =  'Content/ExpenseTracker/icons/ExpenseTracker.png' WHERE [SkuID] = 300001;

Go

-- Staffing Skus --
UPDATE [Billing].[Sku] SET [IconUrl] = 'Content/StaffingManager/icons/StaffingManager.png' WHERE [SkuID] = 400001;

Go
